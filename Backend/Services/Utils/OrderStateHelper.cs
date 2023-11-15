using Services.DTO;
using Shared.Enums;
using System;

namespace Services.Utils {
    public static class OrderStateHelper {
        public static bool IsOrderState(OrderDTO order, OrderStates orderState) {
            if(order.CancelationTime != null) {
                return orderState == OrderStates.CANCELED;
            }
            if(order.RentStart > DateTime.Now) {
                return orderState == OrderStates.PENDING;
            }
            if(order.RentStart <= DateTime.Now && order.RentEnd >= DateTime.Now) {
                return orderState == OrderStates.ACTIVE;
            }
            if(order.RentEnd < DateTime.Now) {
                return orderState == OrderStates.COMPLETED;
            }
            return false;
        }

        public static OrderStates GetOrderState(OrderDTO order) {
            foreach(OrderStates state in Enum.GetValues(typeof(OrderStates))) {
                if(IsOrderState(order, state)) {
                    return state;
                }
            }
            throw new Exception("Order state not found");
        }
    }
}
