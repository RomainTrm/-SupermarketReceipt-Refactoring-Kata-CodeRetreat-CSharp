using Xunit;
using ApprovalTests;
using ApprovalTests.Reporters;

namespace SupermarketReceipt.Test
{
    [UseReporter(typeof(DiffReporter))]
    public class SupermarketTest
    {
        private static readonly Product Apples = new Product("apples", ProductUnit.Kilo);
        private static readonly Product Toothbrush = new Product("toothbrush", ProductUnit.Each);

        private static Teller CreateTeller()
        {
            SupermarketCatalog catalog = new FakeCatalog();
            catalog.AddProduct(Toothbrush, 0.99);
            catalog.AddProduct(Apples, 1.99);

            return new Teller(catalog);
        }

        [Fact]
        public void WithoutDiscount()
        {
            // ARRANGE
            var cart = new ShoppingCart();
            cart.AddItemQuantity(Apples, 2.5);

            // ACT
            var teller = CreateTeller();
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            var printReceipt = new ReceiptPrinter().PrintReceipt(receipt);
            Approvals.Verify(printReceipt);
        }

        [Fact]
        public void With10PercentDiscount()
        {
            // ARRANGE
            var cart = new ShoppingCart();
            cart.AddItemQuantity(Apples, 2.5);
            cart.AddItemQuantity(Toothbrush, 3);

            var teller = CreateTeller();
            teller.AddSpecialOffer(SpecialOfferType.TenPercentDiscount, Toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            var printReceipt = new ReceiptPrinter().PrintReceipt(receipt);
            Approvals.Verify(printReceipt);
        }

        [Fact]
        public void WithOfferThreeForTwo()
        {
            // ARRANGE
            var cart = new ShoppingCart();
            cart.AddItemQuantity(Toothbrush, 3);

            var teller = CreateTeller();
            teller.AddSpecialOffer(SpecialOfferType.ThreeForTwo, Toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            var printReceipt = new ReceiptPrinter().PrintReceipt(receipt);
            Approvals.Verify(printReceipt);
        }
        
        [Fact]
        public void WithOfferTwoForAmount()
        {
            // ARRANGE
            var cart = new ShoppingCart();
            cart.AddItemQuantity(Toothbrush, 3);

            var teller = CreateTeller();
            teller.AddSpecialOffer(SpecialOfferType.TwoForAmount, Toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            var printReceipt = new ReceiptPrinter().PrintReceipt(receipt);
            Approvals.Verify(printReceipt);
        }
        
        [Fact]
        public void WithOfferFiveForAmount()
        {
            // ARRANGE
            var cart = new ShoppingCart();
            cart.AddItemQuantity(Toothbrush, 7);

            var teller = CreateTeller();
            teller.AddSpecialOffer(SpecialOfferType.FiveForAmount, Toothbrush, 10.0);

            // ACT
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            var printReceipt = new ReceiptPrinter().PrintReceipt(receipt);
            Approvals.Verify(printReceipt);
        }
        
        [Fact]
        public void AddMultipleTimesSameProduct()
        {
            // ARRANGE
            var cart = new ShoppingCart();
            cart.AddItemQuantity(Apples, 2.5);
            cart.AddItemQuantity(Apples, 1.5);
            cart.AddItemQuantity(Apples, 3);

            // ACT
            var teller = CreateTeller();
            var receipt = teller.ChecksOutArticlesFrom(cart);

            // ASSERT
            var printReceipt = new ReceiptPrinter().PrintReceipt(receipt);
            Approvals.Verify(printReceipt);
        }
    }
}