namespace Airliner.Classes
{
    /// <summary>
    /// Перечесление возможных количеств двигателей
    /// </summary>
    public enum TypesQuantityOfEngine
    {
        Two = 2,
        Four = 4,
        Six = 6,
        Eight = 8
    }

    /// <summary>
    /// Структура самолёта
    /// </summary>
    public class InfAirliner
    {
        /// <summary>
        /// Длина самолёта
        /// </summary>
        public double LengthOfAircraft;

        /// <summary>
        /// Диаметр фюзеляжа
        /// </summary>
        public double FuselageDiameter;

        /// <summary>
        /// Размах крыла
        /// </summary>
        public double Wingspan;

        /// <summary>
        /// Горизонтальное расположение крыла
        /// </summary>
        public double HorizontalPositionWing;

        /// <summary>
        /// Вертикальное расположение
        /// </summary>
        public double VerticalPositionWing;

        /// <summary>
        /// Угол стреловидности крыла
        /// </summary>
        public double SweepbackAngle;

        /// <summary>
        /// Высота киля
        /// </summary>
        public double HeightOfKeel;

        /// <summary>
        /// Длина вертикального стабилизатора
        /// </summary>
        public double LengthOfHorizontalStabilizer;

        /// <summary>
        /// Количество двигал=телей
        /// </summary>
        public TypesQuantityOfEngine TypeQuantityOfEngine;

        /// <summary>
        /// Конструктор с входными параметрами размеров частей самолёта
        /// </summary>
        /// <param name="lengthOfAircraft"></param>
        /// <param name="fuselageDiameter"></param>
        /// <param name="wingspan"></param>
        /// <param name="horizontalPositionWing"></param>
        /// <param name="verticalPositionWing"></param>
        /// <param name="sweepbackAngle"></param>
        /// <param name="heightOfKeel"></param>
        /// <param name="lengthOfHorizontalStabilizer"></param>
        /// <param name="typeQuantityOfEngine"></param>
        public InfAirliner(double lengthOfAircraft, double fuselageDiameter, double wingspan,
                double horizontalPositionWing, double verticalPositionWing, double sweepbackAngle,
                double heightOfKeel, double lengthOfHorizontalStabilizer, TypesQuantityOfEngine typeQuantityOfEngine)
        {
            LengthOfAircraft = lengthOfAircraft;
            FuselageDiameter = fuselageDiameter;
            Wingspan = wingspan;
            HorizontalPositionWing = horizontalPositionWing;
            VerticalPositionWing = verticalPositionWing;
            SweepbackAngle = sweepbackAngle;
            HeightOfKeel = heightOfKeel;
            LengthOfHorizontalStabilizer = lengthOfHorizontalStabilizer;
            TypeQuantityOfEngine = typeQuantityOfEngine;
        }
    }
}
