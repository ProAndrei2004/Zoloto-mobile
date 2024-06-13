using Microsoft.Maui.Controls;

namespace Zoloto
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            Prod.ItemsSource = new List<Product>
            {
                new Product {Name="Цепочка золотая", ImagePath="https://w7.pngwing.com/pngs/868/826/png-transparent-necklace-chain-gold-jewellery-metal-necklace.png", Description = "Цена от 20000" },
                new Product {Name="Цепочка серебряная", ImagePath="https://png.klev.club/uploads/posts/2024-03/png-klev-club-p-tsepochka-png-1.png", Description = "Цена от 8000" },
                new Product {Name="Серьги золотые", ImagePath="https://w7.pngwing.com/pngs/98/122/png-transparent-earring-body-jewellery-jewellery-miscellaneous-body-jewellery-body-jewelry.png", Description = "Цена от 25000" },
                new Product {Name="Кольцо с обручльным камнем", ImagePath="https://w7.pngwing.com/pngs/964/161/png-transparent-engagement-ring-wedding-ring-jewellery-gold-jewellery-miscellaneous-gemstone-ring.png", Description = "Цена от 35999" },
                new Product {Name="Кольцо обручальное", ImagePath="https://w7.pngwing.com/pngs/1013/131/png-transparent-wedding-invitation-wedding-ring-engagement-ring-groom-ring-wedding-people.png", Description = "Цена от 12000" },
            };
            // определяем шаблон данных
            Prod.ItemTemplate = new DataTemplate(() =>
            {
                Label header = new Label
                {
                    FontAttributes = FontAttributes.Bold,
                    HorizontalTextAlignment = TextAlignment.Center,
                    FontSize = 18
                };
                header.SetBinding(Label.TextProperty, "Name");

                Image image = new Image { WidthRequest = 150, HeightRequest = 150 };
                image.SetBinding(Image.SourceProperty, "ImagePath");

                Label description = new Label { HorizontalTextAlignment = TextAlignment.Center };
                description.SetBinding(Label.TextProperty, "Description");

                StackLayout stackLayout = new StackLayout() { header, image, description };
                Frame frame = new Frame();
                frame.Content = stackLayout;
                return frame;
            });
        }


    }
}
