using System.Collections.Generic;
using DarkEnergy.Inventory;

namespace DarkEnergy.Trading
{
    public class Vendor
    {
        private List<int> idList;

        public List<IItem> Items { get; protected set; }
        public List<Price> Prices { get; protected set; }

        public float TradeModifier { get; set; }

        /// <summary>
        /// Buys and sells items.
        /// </summary>
        /// <param name="itemIdList">A list of item ids.</param>
        /// <param name="stackList">Defines the stack size of every item.</param>
        /// <param name="priceList">Defines the price of every item.</param>
        /// <param name="currencyList">Defines the currency used to purchase items.</param>
        /// <param name="priceModifier">Defines the vendor-assigned price of the items in stock relative to their standard value.</param>
        /// <param name="tradeModifier">Defines the price the vendor is willing to pay to purchase items sold by the player.</param>
        public Vendor(List<int> itemIdList, List<int> stackList, List<int> priceList, List<Currency> currencyList, float priceModifier, float tradeModifier)
        {
            TradeModifier = tradeModifier;
            LoadItems(itemIdList, stackList);
            CalculatePrices(priceList, currencyList, priceModifier);
        }

        protected void LoadItems(List<int> itemIdList, List<int> stackList)
        {
            idList = itemIdList;

            Items = new List<IItem>();

            for (var i = 0; i < idList.Count; ++i)
            {
                var id = idList[i];
                var stackSize = stackList[i];
                var item = DataManager.Load<IItem>(id);

                if (item == null)
                {
                    ExceptionManager.Log("Cannot load an item with id " + id.ToString() + ".");
                }
                else
                {
                    if (stackSize < 1)
                    {
                        ExceptionManager.Log("Invalid item stack size.");
                        stackSize = 1;
                    }

                    item.Count = stackSize;
                    Items.Add(item);
                }
            }
        }

        protected void CalculatePrices(List<int> priceList, List<Currency> currencyList, float priceModifier)
        {
            if (Items != null)
            {
                Prices = new List<Price>();

                for (var i = 0; i < Items.Count; ++i)
                {
                    int price = (priceList[i] > 0 ? priceList[i] : (int)(Items[i].Value * priceModifier));
                    Prices.Add(new Price(price, currencyList[i]));
                }
            }
            else
            {
                ExceptionManager.Log("Cannot calculate prices because no items have been loaded.");
            }
        }

        public int GetId(IItem item)
        {
            var index = Items.IndexOf(item);

            if (index >= 0)
            {
                if (index < idList.Count)
                {
                    return idList[index];
                }
                else
                {
                    ExceptionManager.Log("Cannot get item id because there are fewer entries in idList than there are in Items.");
                    return 0;                    
                }
            }
            else
            {
                ExceptionManager.Log("Cannot get item id because the item does not exist in Items.");
                return 0;
            }
        }

        public int GetId(int index)
        {
            if (0 <= index && index < idList.Count)
            {
                return idList[index];
            }
            else
            {
                ExceptionManager.Log("Cannot get item id because the specified index is invalid.");
                return 0;
            }
        }

        public Price PriceOf(IItem item)
        {
            var index = Items.IndexOf(item);
            return PriceOf(index);
        }

        public Price PriceOf(int index)
        {
            if (0 <= index && index < idList.Count)
            {
                return Prices[index];
            }
            else
            {
                ExceptionManager.Log("Cannot get item price because the specified index is invalid.");
                return new Price(0, Currency.Coins);
            }
        }
    }
}
