using System.Threading.Tasks;
using System.Windows.Input;
using System.Collections.Generic;
using Xamarin.Forms;
using FFImageLoading.Work;
using FFImageLoading.Forms;
using FFImageLoading.Transformations;

namespace ImageButtonDemo.Controls
{
    public class ImageButton : CachedImage
    {
        public static readonly BindableProperty CommandProperty = BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(ImageButton), null);
        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }


        public static readonly BindableProperty CommandParameterProperty = BindableProperty.Create(nameof(CommandParameter), typeof(object), typeof(ImageButton), null);
        public object CommandParameter
        {
            get { return (object)GetValue(CommandParameterProperty); }
            set { SetValue(CommandParameterProperty, value); }
        }


        public static readonly BindableProperty IsDisabledProperty = BindableProperty.Create(nameof(IsDisabled), typeof(bool), typeof(ImageButton), false, propertyChanged: OnDisabledChanged);
        public bool IsDisabled
        {
            get { return (bool)GetValue(IsDisabledProperty); }
            set { SetValue(IsDisabledProperty, value); }
        }

        private ICommand TransitionCommand
        {
            get
            {
                return new Command(async () => {
                    if (!IsDisabled) {
                        try {
                            Transformations = new List<ITransformation> { new ColorSpaceTransformation(FFColorMatrix.PolaroidColorMatrix) };
                            await Task.Delay(100);
                            Transformations = new List<ITransformation> { };
                        } catch {
                            // nothing to do
                        }

                        if (Command != null) {
                            Command.Execute(CommandParameter);
                        }
                    }
                });

            }
        }

        public ImageButton()
        {
            Initialize();
        }

        public void Initialize()
        {
            DownsampleToViewSize = true;
            Aspect = Aspect.AspectFit;

            GestureRecognizers.Add(new TapGestureRecognizer() {
                Command = TransitionCommand
            });
        }

        private static void OnDisabledChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var imageButton = (bindable as ImageButton);

            try {
                imageButton.Transformations = new List<ITransformation> { };
                if (imageButton.IsDisabled) {
                    imageButton.Transformations = new List<ITransformation> { new ColorSpaceTransformation(Color.FromHex("#424242").ColorToMatrix()) };
                }
            } catch (System.Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
    }
}
