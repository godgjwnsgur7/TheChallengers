using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing;
using UnityEngine.UI;

namespace FGPlatform.Purchase
{
    public class IAPProduct
    {
        public string ID = "";
        public string AOS_ID = "";
        public ProductType Type = ProductType.Consumable;
        public long Price = 0;

        public IAPProduct(string id, string aos_id, ProductType type, long price)
        {
            ID = id;
            AOS_ID = aos_id;
            Type = type;
            Price = price;
        }
    }

    public interface CoffeeMachine
    {
        void Init();
        bool Purchase();
        bool IsPurchased();
    }

    /// <summary>
    /// 우선 안드로이드만 지원하기로 함
    /// </summary>

    public class IAPController : IStoreListener, CoffeeMachine
    {
        private IStoreController storeController = null;
        private IExtensionProvider provider = null;

        private ConfigurationBuilder builder = null;
        private StandardPurchasingModule module = null;

        private readonly IAPProduct productInfo = new IAPProduct("커피", "Coffee", ProductType.Consumable, 1000);

        public bool IsValid =>
            storeController != null
            && provider != null
            && builder != null
            && module != null;

        public void Init()
        {
            module = StandardPurchasingModule.Instance();
            builder = ConfigurationBuilder.Instance(module);

            builder.AddProduct(productInfo.ID, productInfo.Type, new IDs()
            {
                { productInfo.AOS_ID, GooglePlay.Name}
            });

            UnityPurchasing.Initialize(this, builder);
        }

        public bool Purchase()
        {
            if (!IsValid)
                return false;

            var product = storeController.products.WithID(productInfo.ID);
            if (product == null || !product.availableToPurchase)
                return false;

            storeController.InitiatePurchase(product);
            return true;
        }

        public bool IsPurchased()
        {
            if (!IsValid)
                return false;

            var product = storeController.products.WithID(productInfo.ID);
            if (product == null)
                return false;

            return product.hasReceipt;
        }

        public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
        {
            storeController = controller;
            provider = extensions;

            Debug.Log("유니티 Billing IAP 초기화 성공");
        }

        public void OnInitializeFailed(InitializationFailureReason error)
        {
            Debug.LogError($"유니티 Billing IAP 초기화 실패 [{error}]");
        }

        public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
        {
            if (purchaseEvent.purchasedProduct.definition.id == productInfo.ID &&
                purchaseEvent.purchasedProduct.definition.storeSpecificId == productInfo.AOS_ID)
            {
                Debug.Log($"[{purchaseEvent.purchasedProduct.definition.id}] 구매 성공");
            }

            return PurchaseProcessingResult.Complete;
        }

        void IStoreListener.OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
        {
            Debug.LogError($"[{product.definition.id}] 구매 실패 [{failureReason}]");
        }
    }
}


