using BaseLibrary.DTOs;
using Blazored.LocalStorage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static BaseLibrary.DTOs.CartDTO;
using static BaseLibrary.Models.Cart;


namespace ClientUserLibrary.Services.CartService
{
    public class CartService
    {
        private ILocalStorageService _localStorage;
        public event Action OnCartChanged;
        private const string CartKey = "cartItems";

        public CartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task AddItemToCart(ProductCartItem product, int quantity)
        {
            var cart = await _localStorage.GetItemAsync<List<CartLine>>(CartKey) ?? new List<CartLine>();
            var item = cart.Find(i => i.productItem.Id == product.Id && i.productItem.size == product.size && i.productItem.color == product.color);
            if (item != null)
            {
                item.Quantity += quantity;
            }
            else
            {
                cart.Add(new CartLine { productItem = product, Quantity = quantity });
            }
            await _localStorage.SetItemAsync(CartKey, cart);
            OnCartChanged?.Invoke();
        }


        public async Task RemoveItemFromCart(ProductCartItem product)
        {
            var cart = await _localStorage.GetItemAsync<List<CartLine>>(CartKey) ?? new List<CartLine>();
            var item = cart.Find(i => i.productItem.Id == product.Id);
            if (item != null)
            {
                cart.Remove(item);
            }
            await _localStorage.SetItemAsync(CartKey, cart);
            OnCartChanged?.Invoke();
        }

        public async Task<List<CartLine>> GetCartItems()
        {
            return await _localStorage.GetItemAsync<List<CartLine>>(CartKey) ?? new List<CartLine>();
        }

        public async Task<int> GetCartItemCount()
        {
            var cart = await _localStorage.GetItemAsync<List<CartLine>>(CartKey) ?? new List<CartLine>();
            return cart.Sum(item => item.Quantity);
        }
    }



}
