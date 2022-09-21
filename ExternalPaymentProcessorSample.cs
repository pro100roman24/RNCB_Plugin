using Newtonsoft.Json;
using Resto.Front.Api;
using Resto.Front.Api.Attributes.JetBrains;
using Resto.Front.Api.Data.Cheques;
using Resto.Front.Api.Data.Orders;
using Resto.Front.Api.Data.Organization;
using Resto.Front.Api.Data.Payments;
using Resto.Front.Api.Data.Security;
using Resto.Front.Api.Data.View;
using Resto.Front.Api.Exceptions;
using Resto.Front.Api.Extensions;
using Resto.Front.Api.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;


namespace RNCB_Plugin
{
    internal class ExternalPaymentProcessorSample : IPaymentProcessor, IDisposable
    {
        public ExternalPaymentProcessorSample()
        {
        }

        public string PaymentSystemKey => Plugin.Name;

        public string PaymentSystemName => PaymentSystemKey;

        public bool CanPaySilently(decimal sum, Guid? orderId, Guid paymentTypeId, IPaymentDataContext context)
        {
            return false;
        }

        public void CollectData(Guid orderId, Guid paymentTypeId, [NotNull] IUser cashier, IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context)
        {
            
        }

        public void Dispose()
        {
            
        }

        public void EmergencyCancelPayment(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, IViewManager viewManager, IPaymentDataContext context)
        {
            throw new NotImplementedException();
        }

        public void EmergencyCancelPaymentSilently(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, IPaymentDataContext context)
        {
            throw new NotImplementedException();
        }

        public void OnPaymentAdded([NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, [NotNull] IUser cashier, [NotNull] IOperationService operationService, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            
        }

        public bool OnPreliminaryPaymentEditing([NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, [NotNull] IUser cashier, [NotNull] IOperationService operationService, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            throw new NotImplementedException();
        }

        public void Pay(decimal sum, [NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, [NotNull] IOperationService operationService, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            
        }

        public void PaySilently(decimal sum, [NotNull] IOrder order, [NotNull] IPaymentItem paymentItem, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, IPaymentDataContext context)
        {
            throw new NotImplementedException();
        }

        public void ReturnPayment(decimal sum, Guid? orderId, Guid paymentTypeId, Guid transactionId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, [NotNull] IViewManager viewManager, IPaymentDataContext context)
        {
            
        }

        public void ReturnPaymentWithoutOrder(decimal sum, Guid? orderId, Guid paymentTypeId, [NotNull] IPointOfSale pointOfSale, [NotNull] IUser cashier, IReceiptPrinter printer, [NotNull] IViewManager viewManager)
        {
            
        }
    }
}