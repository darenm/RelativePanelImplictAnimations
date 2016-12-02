using System;
using System.Numerics;
using System.Threading.Tasks;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Robmikh.CompositionSurfaceFactory;

namespace RelativePanelImplicitAnimations
{
    public class VisualControl : UserControl
    {
        public static readonly DependencyProperty ImageUriProperty = DependencyProperty.Register(
            "ImageUri", typeof(Uri), typeof(VisualControl), new PropertyMetadata(default(Uri), ImageUriChanged));

        private readonly Compositor _compositor;

        public VisualControl()
        {
            _compositor = ElementCompositionPreview.GetElementVisual(this).Compositor;
            Visual = _compositor.CreateSpriteVisual();
            ElementCompositionPreview.SetElementChildVisual(this, Visual);
            SizeChanged +=
                (sender, args) =>
                {
                    Visual.Size = new Vector2((float) args.NewSize.Width, (float) args.NewSize.Height);
                };
        }

        public Uri ImageUri
        {
            get { return (Uri) GetValue(ImageUriProperty); }
            set { SetValue(ImageUriProperty, value); }
        }

        public SpriteVisual Visual { get; }

        private static void ImageUriChanged(DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            var instance = (VisualControl) dependencyObject;
            var unused = instance.LoadImage(dependencyPropertyChangedEventArgs.NewValue as Uri);
        }

        public async Task LoadImage(Uri imageUri)
        {
            try
            {
                var sf = SurfaceFactory.CreateFromCompositor(_compositor);
                var surface = await sf.CreateSurfaceFromUriAsync(imageUri);
                var brush = _compositor.CreateSurfaceBrush(surface);
                brush.Stretch = CompositionStretch.Uniform;
                Visual.Brush = brush;
                Visual.Size = new Vector2((float) ActualWidth, (float) ActualHeight);
            }
            catch (Exception ex)
            {
                var m = ex.Message;
            }
        }
    }
}