using System;
using System.Windows;
using Airliner.Classes;

namespace Airliner
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        //public DataContainer DataContainer;

        public InfAirliner InfAirliner;
        public const double VertPosWing = 1.5;
        public Manager Manager = new Manager();
        public MainWindow()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Считавние размеров частей самолёта с "Lable"
        /// </summary>
        public void ChangeInfAirliner()
        {
            InfAirliner = new InfAirliner(Convert.ToDouble(LableLengthAir.Content),
                Convert.ToDouble(LableDiameterFus.Content),
                Convert.ToDouble(LableWingpan.Content),
                Convert.ToDouble(LableHorizPosWing.Content),
                VertPosWing,
                Convert.ToDouble(LableSweepbackAng.Content),
                Convert.ToDouble(LableHeidhtOfKeel.Content),
                Convert.ToDouble(LableLengthOfHorizStabil.Content),
                (TypesQuantityOfEngine)Convert.ToInt32(QuantityEngine.Text));
        }

        /// <summary>
        /// Кнопка запуска сознания модели
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ChangeInfAirliner();
            Manager.OpenKompas3D();
            Manager.InitializeModel(InfAirliner);
        }

        /// <summary>
        /// Изменение мин и макс изменяемых значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderLengthAir_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderHorizPosWing.Minimum = Math.Round((SliderLengthAir.Value * 0.33) + (SliderDiameterFus.Value * 0.9375), 1);
            SliderHorizPosWing.Maximum = Math.Round((SliderLengthAir.Value * 0.78) - (SliderDiameterFus.Value * 0.9375), 1);
            SliderWingpan.Minimum = Math.Round(SliderWingpan.Maximum * 0.5, 1);
            SliderHeidhtOfKeel.Minimum = Math.Round(SliderWingpan.Value * 0.15, 1);
            SliderHeidhtOfKeel.Maximum = Math.Round(SliderWingpan.Value * 0.25, 1);
            SliderLengthOfHorizStabil.Minimum = Math.Round(SliderWingpan.Value * 0.3, 1);
            SliderLengthOfHorizStabil.Maximum = Math.Round(SliderWingpan.Value * 0.5, 1);
            SliderDiameterFus.Minimum = Math.Round(SliderLengthAir.Value * 0.083, 1);
            SliderDiameterFus.Maximum = Math.Round(SliderLengthAir.Value * 0.13, 1);
        }

        /// <summary>
        /// Изменение мин и макс изменяемых значений
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SliderWingpan_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            SliderLengthOfHorizStabil.Minimum = Math.Round(SliderWingpan.Value * 0.3, 1);
            SliderLengthOfHorizStabil.Maximum = Math.Round(SliderWingpan.Value * 0.5, 1);
            SliderHeidhtOfKeel.Minimum = Math.Round(SliderWingpan.Value * 0.15, 1);
            SliderHeidhtOfKeel.Maximum = Math.Round(SliderWingpan.Value * 0.25, 1);
        }
    }
}
