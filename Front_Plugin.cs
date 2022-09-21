using Resto.Front.Api;
using Resto.Front.Api.Attributes;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Data.Common;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Data.Payments;
using Resto.Front.Api.OperationContexts;
using Resto.Front.Api.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using RequestQR;
using ResponseQR;
using Resto.Front.Api.Data.Cheques;
using System.Xml.Linq;
using Resto.Front.Api.Data.Print;
using Resto.Front.Api.Exceptions;
using Resto.Front.Api.Data.Organization;
using Resto.Front.Api.Data.Screens;
using Resto.Front.Api.Data.Device;
using Resto.Front.Api.Data.Security;
using Resto.Front.Api.Extensions;
using Resto.Front.Api.Data.Brd;
using Resto.Front.Api.Data.View;

namespace RNCB_Plugin
{
    [UsedImplicitly]
    [PluginLicenseModuleId(21016318)]

    public sealed class Front_Plugin : IFrontPlugin
    {
        public IntPtr front;

        private readonly CompositeDisposable subscriptions;
        public Front_Plugin()
        {
            Plugin.Log_Mess_Warn("Плагин {0} запускается.", Plugin.Name);
            subscriptions = new CompositeDisposable();
            //var paymentSystem = new ExternalPaymentProcessorSample();
            //subscriptions.Add(paymentSystem);
            //try
            //{
            //    subscriptions.Add(PluginContext.Operations.RegisterPaymentSystem(paymentSystem, true));

            //}
            //catch (LicenseRestrictionException ex)
            //{
            //    PluginContext.Log.Warn(ex.Message);
            //    return;
            //}
            //catch (PaymentSystemRegistrationException ex)
            //{
            //    PluginContext.Log.Warn($"Payment system '{paymentSystem.PaymentSystemKey}': '{paymentSystem.PaymentSystemName}' wasn't registered. Reason: {ex.Message}");
            //    return;
            //}

            //PluginContext.Log.Info($"Payment system '{paymentSystem.PaymentSystemKey}': '{paymentSystem.PaymentSystemName}' was successfully registered on server.");

            //subscriptions.Push(new Tester());
            var f = Config.Instance.TerminalID;
            PluginContext.Notifications.BillChequePrinting.Subscribe(AddBillChequeExtensions);
            PluginContext.Notifications.NavigatingToPaymentScreen.Subscribe(NavigatingToPaymentScreen);
            Plugin.Log_Mess_Info("Плагин {0} успешно запущен.", Plugin.Name);

            PluginContext.Notifications.ScreenChanged.Subscribe(ScreenChanged);

           // PluginContext.Operations.AddButtonToPluginsMenu("Тест кнопка", x => btn(x.vm, x.printer));

        }

        private void btn(IViewManager vm, IReceiptPrinter printer)
        {
            ExtendedInputDialogSettings settings = new ExtendedInputDialogSettings() { EnablePhone = true };
            var result = vm.ShowExtendedInputDialog("Введите номер телефона", "Телефон", settings);
            var phone = result as PhoneInputDialogResult;
            var client = PluginContext.Operations.TryGetClientByPhone(PluginContext.Operations.GetCredentials(), phone.PhoneNumber );
            List<EmailDto> emailDto = new List<EmailDto>();
            emailDto.Add( new EmailDto() { EmailValue = vm.ShowKeyboard("Введите емейл"), IsMain = true });
            PluginContext.Operations.ChangeClientEmails( emailDto, client, PluginContext.Operations.GetCredentials());
        }

        private void ScreenChanged(IScreen obj)
        {
            var Screen = obj;
            //Plugin.Log_Mess_Info("Сработал ScreenChanged");
        }

        private void NavigatingToPaymentScreen((IOrder order, IPointOfSale pos, IOperationService os, IViewManager vm, INavigatingToPaymentScreenOperationContext context) obj)
        {
            var order = obj.order;
            var os = obj.os;
            var vm = obj.vm;
            Plugin.Log_Info($"Проверяем оплату заказа {order.Number}");

            var externalId = PluginContext.Operations.TryGetOrderExternalDataByKey(order, "externalId");

            if (externalId == null)
            {
                return;
            }

            try
            {
                if (order.Payments.Where(x => (x.Type.Name == Config.Instance.PayTypeName)).Any())
                {
                    Plugin.Log_Mess_Info($"Оплата \"{Config.Instance.PayTypeName}\" уже внесена в заказ №{order.Number}");
                    return;
                }
                RequestQRstatus.RegQrBody qrBody = new RequestQRstatus.RegQrBody()
                {
                    header = new RequestQRstatus.Header()
                    {
                        protocol = new RequestQRstatus.Protocol()
                        {
                            name = "solar-ws",
                            version = "2.0"
                        },
                        messageId = Guid.NewGuid().ToString(),
                        messageDate = DateTime.Now,
                        originator = new RequestQRstatus.Originator()
                        {
                            system = "sslUser"
                        },
                        responseParams = new RequestQRstatus.ResponseParams()
                        {
                            includes = new RequestQRstatus.Includes()
                            {
                                include = new List<RequestQRstatus.Include>() { new RequestQRstatus.Include() { data = "so.nspkDocumentStatus", include = "true" } }

                            }
                        }

                    },
                    body = new RequestQRstatus.Body()
                    {
                        paymentCodeDocumentRef = new RequestQRstatus.PaymentCodeDocumentRef()
                        {
                            externalId = externalId
                        }
                    }
                };

                var body = JsonConvert.SerializeObject(qrBody);

                var result = CheckQR(body);

                //var editsession = PluginContext.Operations.CreateEditSession();
                //editsession.AddOrderExternalData("externalId", result.body.paymentCodeDocument.externalId, true, order);

                if (result.body.paymentCodeDocument.status == "FULLY_PAID")
                {
                    var editsession = os.CreateEditSession();
                    var paymentType = os.GetPaymentTypes().Where(x => (x.Name == Config.Instance.PayTypeName)).First();
                    editsession.AddExternalPaymentItem((decimal)result.body.paymentCodeDocument.amounts.transactionAmount.amount, true, null, null, paymentType, order);
                    os.SubmitChanges(os.GetCredentials(), editsession);

                    if (Config.Instance.ShowOkPopupAboutPay)
                        vm.ShowOkPopup("СБП - РНКБ", $"Заказ оплачен через систему СБП, оплата \"{paymentType.Name}\"" +
                        $" на сумму {(decimal)result.body.paymentCodeDocument.amounts.transactionAmount.amount}р. автоматически добавлена в заказ.");
                    else
                        Plugin.Log_Mess_Info($"Заказ оплачен через систему СБП, оплата \"{paymentType.Name}\"" +
                            $" на сумму {(decimal)result.body.paymentCodeDocument.amounts.transactionAmount.amount}р. автоматически добавлена в заказ.");
                }
                else
                {
                    Plugin.Log_Info($"Заказ № {order.Number} не оплачен через СБП, текущий статус: {result.body.paymentCodeDocument.status}");
                }
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error(ex.ToString());
            }
            return;

        }

        private BillCheque AddBillChequeExtensions(Guid Id)
        {
            var order = PluginContext.Operations.GetOrderById(Id);
            Plugin.Log_Info($"Печать пречека на заказ {order.Number}");
            try
            {
                if (order.ResultSum == 0M)
                    return (BillCheque)null;

                var qrurl = "";
                qrurl = PluginContext.Operations.TryGetOrderExternalDataByKey(order, "qrurl");
                RespQR result = new RespQR();

                if (qrurl == null)
                {
                    Plugin.Log_Info("отправляем запрос на генерацию ссылки на оплату заказа");

                    RegQrBody qrBody = new RegQrBody()
                    {
                        header = new RequestQR.Header()
                        {
                            protocol = new RequestQR.Protocol()
                            {
                                name = "solar-ws",
                                version = "2.0"
                            },
                            messageId = order.Id.ToString(),
                            messageDate = DateTime.Now,
                            originator = new RequestQR.Originator()
                            {
                                system = "sslUser"
                            }

                        },
                        body = new RequestQR.Body()
                        {
                            paymentCodeDocument = new RequestQR.PaymentCodeDocument()
                            {
                                terminalRef = new TerminalRef()
                                {
                                    parameters = new Parameters1()
                                    {
                                        number = Config.Instance.TerminalID
                                    }
                                },
                                providerRef = new ProviderRef()
                                {
                                    parameters = new Parameters2()
                                    {
                                        code = "NSPK_QR"
                                    }
                                },
                                amounts = new RequestQR.Amounts()
                                {
                                    transactionAmount = new RequestQR.TransactionAmount()
                                    {
                                        amount = order.ResultSum.ToString().Replace(',', '.'),
                                        currency = "643"
                                    }
                                },
                                description = $"Оплата заказа №{order.Number}"
                            }
                        }
                    };

                    Plugin.Log_Info("Собрали qrBody");

                    var body = JsonConvert.SerializeObject(qrBody);

                    Plugin.Log_Info("Сериализация qrBody прошла успешно");

                    result = GenerateQR(body);

                    var editsession = PluginContext.Operations.CreateEditSession();
                    editsession.AddOrderExternalData("externalId", result.body.paymentCodeDocument.externalId, true, order);
                    editsession.AddOrderExternalData("qrurl", result.body.paymentCodeDocument.payload, true, order);
                    PluginContext.Operations.SubmitChanges(PluginContext.Operations.GetCredentials(), editsession);
                    qrurl = result.body.paymentCodeDocument.payload;
                    Plugin.Log_Info($"Ссылка готова: {qrurl}");
                }

                if (qrurl != null)
                {
                    XElement xelement = new XElement((XName)"doc");
                    XElement content = new XElement((XName)"qrcode", qrurl);
                    content.SetAttributeValue((XName)"size", Config.Instance.qrsize);

                    XElement content2 = new XElement((XName)"center", "Отсканируйте QR код для оплаты через СБП");
                    XElement content3 = new XElement((XName)"f0", content2);

                    xelement.Add((object)content);
                    xelement.Add((object)content3);

                    if (Config.Instance.print_on_receipt_printer)
                        PluginContext.Operations.Print(PluginContext.Operations.GetReportPrinter(), (Document)xelement);
                    if (!Config.Instance.print_on_precheque)
                        return (BillCheque)null;
                    PluginContext.Log.Info("Печатаем QR код на чек: " + qrurl);
                    return new BillCheque() { AfterFooter = xelement };
                }
            }
            catch (Exception ex)
            {
                PluginContext.Log.Error(ex.ToString());
            }
            return (BillCheque)null;


        }

        private void OrderScreenButton(IOrder order, IOperationService os, IViewManager vm)
        {
            //var windowThread = new Thread(EntryPoint);
            //windowThread.SetApartmentState(ApartmentState.STA);
            //windowThread.Start();
            //while (windowThread.IsAlive) { }


            //var RequestCRM = new HttpRequest();
            //RequestCRM.KeepAliveTimeout = 3000;
            //RequestCRM.ReadWriteTimeout = 3000;
            //RequestCRM.ConnectTimeout = 3000;

            //RequestCRM.SslCertificateValidatorCallback = new System.Net.Security.RemoteCertificateValidationCallback() { Method = Method.Post, Target =  };


            //var Response = RequestCRM.Post($"https://195.200.209.97:16443/solar-proc-payment-codes-gateway/ext/term-pc-json-api/registerPaymentCodeDocument", body, "application/json").ToString();
            //var OrdInfo = JsonConvert.DeserializeObject<shortLink>(Response);
            //Plugin.Log_Info($"Ссылка сокращена - {OrdInfo.short_url}");

        }

        public string PostData(string DataToPost, string URL)
        {
            string result = "";
            string strPost = DataToPost;
            StreamWriter myWriter = null;
            HttpWebRequest objRequest = (HttpWebRequest)WebRequest.Create(URL);
            objRequest.Method = "POST";
            objRequest.ContentLength = strPost.Length;
            objRequest.ContentType = "application/json";
            objRequest.ClientCertificates.Add(new X509Certificate2(@"C:\Users\Роман\Documents\Ялта Интурист\сертификаты новые\client_keystore.p12", "SrCVBSuYkL"));
            try
            {
                myWriter = new StreamWriter(objRequest.GetRequestStream());
                myWriter.Write(strPost);
            }
            catch (Exception e)
            {
                //
            }
            finally
            {
                myWriter.Close();
            }
            HttpWebResponse objResponse = (HttpWebResponse)objRequest.GetResponse();
            using (StreamReader sr =
            new StreamReader(objResponse.GetResponseStream()))
            {
                result = sr.ReadToEnd();
                // Close and clean up the StreamReader
                sr.Close();
            }
            return result;
        }


        public static RespQR CheckQR(string DataToPost)
        {
            try
            {
                CertificateWebClient certificateWebClient = new CertificateWebClient(new X509Certificate2(Config.Instance.CertName, Config.Instance.CertPass));
                certificateWebClient.Headers.Add("Content-Type", "application/json");

                string data = DataToPost;
                certificateWebClient.Encoding = Encoding.UTF8;
                if (Config.Instance.FullLogs)
                    PluginContext.Log.Info(data);
                string str7 = certificateWebClient.UploadString(Config.Instance.rncbURL + "/solar-proc-payment-codes-gateway/ext/term-pc-json-api/getPaymentCodeDocument", data);
                if (Config.Instance.FullLogs)
                    PluginContext.Log.Info(str7);
                var result = JsonConvert.DeserializeObject<RespQR>(str7);
                return result;
            }
            catch (Exception ex)
            {
                PluginContext.Log.Info(ex.ToString());
                if (ex is WebException)
                {
                    WebException webException = ex as WebException;
                    if (webException.Response != null)
                    {
                        Stream responseStream = webException.Response.GetResponseStream();
                        if (responseStream != null)
                            PluginContext.Log.Info(new StreamReader(responseStream).ReadToEnd());
                    }
                }

                return null;
            }
        }

        public static RespQR GenerateQR(string DataToPost)
        {
            //try
            //{
                Plugin.Log_Info("Зашли в GenerateQR");

                CertificateWebClient certificateWebClient = new CertificateWebClient(new X509Certificate2(Config.Instance.CertName, Config.Instance.CertPass));



                Plugin.Log_Info("Сертификат прочитан");
                certificateWebClient.Headers.Add("Content-Type", "application/json");

                string data = DataToPost;
                certificateWebClient.Encoding = Encoding.UTF8;
                Plugin.Log_Info("Отправляем запрос");

            if (Config.Instance.FullLogs)
                PluginContext.Log.Info(DataToPost);

            string str7 = certificateWebClient.UploadString(Config.Instance.rncbURL + "/solar-proc-payment-codes-gateway/ext/term-pc-json-api/registerPaymentCodeDocument", data);
                Plugin.Log_Info("Ответ получен");
                if (Config.Instance.FullLogs)
                    PluginContext.Log.Info(str7);
                var result = JsonConvert.DeserializeObject<ResponseQR.RespQR>(str7);
                return result;
            //}
            //catch (Exception ex)
            //{
            //    PluginContext.Log.Info(ex.ToString());
            //    if (ex is WebException)
            //    {
            //        WebException webException = ex as WebException;
            //        if (webException.Response != null)
            //        {
            //            Stream responseStream = webException.Response.GetResponseStream();
            //            if (responseStream != null)
            //                PluginContext.Log.Info(new StreamReader(responseStream).ReadToEnd());
            //        }
            //    }

            //    return null;
            //}
        }

        public void EntryPoint()
        {
            string s = "aaa";
            int i = 1;
            var window = new Form1(front, s, i);
            window.ShowDialog();
            var calib = window.output;
            //ProductAmount = Convert.ToDecimal(window.amount);
        }

        private bool change(IViewManager vm)
        {
            var settings = new ExtendedInputDialogSettings
            {
                EnableNumericString = true,
                TabTitleNumericString = "Заголовок вкладки для ввода числа"
            };
            var test = vm.ShowExtendedInputDialog("", "", settings);
            return true;
        }

        public void Dispose()
        {
            while (subscriptions.Any())
            {
                //var subscription = subscriptions.Pop();
                try
                {
                     subscriptions.Dispose();
                }
                catch (RemotingException)
                {
                    // nothing to do with the lost connection
                }
            }
            Plugin.Log_Mess_Info("Плагин {0} успешно остановлен.", Plugin.Name);
        }
    }
}
