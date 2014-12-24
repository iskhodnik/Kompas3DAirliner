using System;
using System.Collections.Generic;
using Kompas6API5;
using Kompas6Constants;
using Kompas6Constants3D;

namespace Airliner.Classes
{
    public class CreatingModel
    {
        /// <summary>
        /// Размеры частей самолёта
        /// </summary>
        private readonly InfAirliner _infAirliner;

        /// <summary>
        /// Объект Компас 3D
        /// </summary>
        private readonly KompasObject _kompasObj;

        /// <summary>
        /// Объект документа Компас 3D
        /// </summary>
        private readonly ksDocument3D _doc3D;

        /// <summary>
        /// Коллекция эскизов
        /// </summary>
        private List<ksEntity> _entityCollection;

        /// <summary>
        /// Коллекция эскизов для резкального отражения
        /// </summary>
        private List<ksEntity> _mirrorCopyEntityCollection;

        /// <summary>
        /// 3D модель
        /// </summary>
        private readonly ksPart _part;

        /// <summary>
        /// Коэффициенты создания фюзеляжа самолёта
        /// </summary>
        private readonly double[] _coefFus = { 0.25, 0.0625, 0.5, 0.33, 0.78, 0.458, 0.85, 0.308, 0.93 };
        
        /// <summary>
        /// Коэффициенты создания крыла самолёта
        /// </summary>
        private readonly double[] _coefWing = { 0.25, 0.9375, 0.15625, 0.18, 0.125, 0.515625, 0.1125, 0.345, 0.20625, 0.01875, 0.5};

        /// <summary>
        /// Коэффициенты создания двигателя самолёта
        /// </summary>
        private readonly double[] _coefEngine = { 0.366, 0.12, 0.0917, 0.125, 0.093, 0.115, 0.123 };

        /// <summary>
        /// Коэффициенты создания креплений двигателя к крылу
        /// </summary>
        private readonly double[] _coefMountEng = { 0.5, 0.016, 0.266, 0.366, 0.12, 0.3, 0.416, 0.025, 0.0917 };

        /// <summary>
        /// Коэффициенты создания хвостового оперения самолёта
        /// </summary>
        private readonly double[] _coefTail = { 0.106, 0.069, 0.081, 0.25, 0.023, 0.029, 0.024, 0.289, 0.5, 0.081, 0.255, 0.061, 0.04 };

        /// <summary>
        /// Коэффициенты создания окон самолёта
        /// </summary>
        private readonly double[] _coefWindows = { 0.2625, 0.1875, 0.125, 0.25, 0.95, 0.0125, 0.53, 0.01, 0.18, 0.03, 0.0033, 0.05, 0.416 };

        /// <summary>
        /// Коэффициенты создания шасси самолёта
        /// </summary>
        private readonly double[] _coefChassis = { 0.86, 0.69, 0.108, 0.1385, 0.017, 0.033, 0.24, 1.013, 0.846, 0.915, 0.961, 0.946, 0.806, 0.866, 0.947, 0.902, 0.879, 0.971 };
        
        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="document3D"></param>
        /// <param name="kompas"></param>
        /// <param name="infAir"></param>
        public CreatingModel(ksDocument3D document3D, KompasObject kompas, InfAirliner infAir)
        {
            _doc3D = document3D;
            _kompasObj = kompas;
            _infAirliner = infAir;
            _part = (ksPart)_doc3D.GetPart((short)Part_Type.pTop_Part);
        }
        
        /// <summary>
        /// Создание всех частей модели самолёта
        /// </summary>
        public void CreateModel()
        {
            CreateFus();
            CreateWing();
            CreateEngines();
            CreateTail();
            CreateChassis();
        }

        /// <summary>
        /// Создание фюзиляжа
        /// </summary>
        private void CreateFus()
        {
            _entityCollection = new List<ksEntity>
            {
                // Создание сечения задней части фюзеляжа
                CreateCircle(0, 
                    _infAirliner.FuselageDiameter * _coefFus[0], 
                    _infAirliner.FuselageDiameter * _coefFus[1], 
                    GetPlane(Obj3dType.o3d_planeXOY)),

                // Создание сечения центральных сечений фюзеляжа
                CreateCircle(0, 
                    0, 
                    _infAirliner.FuselageDiameter  * _coefFus[2],
                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * _coefFus[3])),

                CreateCircle(0, 
                    0,
                    _infAirliner.FuselageDiameter  * _coefFus[2], 
                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * _coefFus[4])),

                CreateCircle(0, 
                    0,
                    _infAirliner.FuselageDiameter * _coefFus[5], 
                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * _coefFus[6])),

                CreateCircle(0, 
                    0, 
                    _infAirliner.FuselageDiameter * _coefFus[7], 
                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * _coefFus[8])),

                // Создание сечения передней части самолёта
                CreateCircle(0, 
                    0, 
                    _infAirliner.FuselageDiameter * _coefFus[1], 
                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft))
            };

            // Выдавливание фюзеляжа по сечениям
            ExtrudeLoft(_entityCollection);
            // Скругление заднего конца фюзеляжа
            Rounding(_infAirliner.FuselageDiameter * _coefFus[1], 0);
            // Скругление переднего конца фузеляжа
            Rounding(_infAirliner.FuselageDiameter * _coefFus[1], 0);
            _entityCollection.Clear();
            // Создание окон в фюзеляже
            CreateWindows();
        }

        /// <summary>
        /// Создание крыльев самолёта
        /// </summary>
        private void CreateWing()
        {
            // Создание списка сечений крыла
            _entityCollection = new List<ksEntity>
                {
                    // Создание сечения начального крыла самолёта 
                    CreateEllipse(-(_infAirliner.HorizontalPositionWing), 
                        _infAirliner.FuselageDiameter * _coefWing[0], 
                        _infAirliner.FuselageDiameter * _coefWing[1],
                        _infAirliner.FuselageDiameter * _coefWing[2],
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, 0)),
                    // Создание сечения серединной части крыла самолёта 
                    CreateEllipse(-(_infAirliner.HorizontalPositionWing - (_infAirliner.Wingspan * _coefWing[3] * Math.Tan(_infAirliner.SweepbackAngle * Math.PI / 180))),
                        _infAirliner.FuselageDiameter * _coefWing[4],
                        _infAirliner.FuselageDiameter * _coefWing[5],
                        _infAirliner.FuselageDiameter * _coefWing[6],
                        0, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.Wingspan * _coefWing[3])),
                    // Создание сечения конечной части крыла самолёта
                    CreateEllipse(-(_infAirliner.HorizontalPositionWing - (_infAirliner.Wingspan * _coefWing[10] * Math.Tan(_infAirliner.SweepbackAngle * Math.PI / 180))), 
                        (_infAirliner.FuselageDiameter * _coefWing[0]) - (_infAirliner.FuselageDiameter * _coefWing[7]),
                        _infAirliner.FuselageDiameter * _coefWing[8], _infAirliner.FuselageDiameter * _coefWing[9],
                        0, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.Wingspan * _coefWing[10]))
                };
            // Выдавливание крыла по сечениям
            var entityWing = ExtrudeLoft(_entityCollection);
            // Зеркальное копирование крыла
            CopyBody(entityWing, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0));
            _entityCollection.Clear();
        }
        
        /// <summary>
        /// Создание двигателей
        /// </summary>
        private void CreateEngines()
        {
            // Создание списка созданных двигателей и креплений двигателей для зеркального отображения
            _mirrorCopyEntityCollection = new List<ksEntity>();

            for (var i = 1; i <= (int)_infAirliner.TypeQuantityOfEngine * 0.5; i++) 
            {
                // Создание двигателей и креплений и записывание их в список
                foreach (var entity in CreateEngine(i))
                {
                    _mirrorCopyEntityCollection.Add(entity);
                }
            }
            // Зеркальное отображение двигателей
            CopyBody(_mirrorCopyEntityCollection, GetPlane(Obj3dType.o3d_planeYOZ));
        }

        /// <summary>
        /// Создание одного двигателя
        /// </summary>
        private List<ksEntity> CreateEngine(int curEng)
        {
            // Расчёт вертикального расположения двигателя
            var engPos = ((_infAirliner.Wingspan - _infAirliner.FuselageDiameter) * 0.5) / (((int)_infAirliner.TypeQuantityOfEngine / 2) + 1);
            // Расчёт горизонтального расположения двигателя
            var horizLocationEng = (_infAirliner.HorizontalPositionWing - ((_infAirliner.Wingspan * 0.5 - (engPos * curEng)) * Math.Tan(_infAirliner.SweepbackAngle * Math.PI / 180)));

            // Создание списка сечений двигателя
            _entityCollection = new List<ksEntity>
                {
                    // Создание сечения начального двигателя 
                    CreateCircle((_infAirliner.Wingspan * 0.5) - (engPos * curEng), 
                        - ((_infAirliner.VerticalPositionWing * _coefEngine[0]) + (engPos * curEng) * _coefEngine[1]), 
                        _infAirliner.FuselageDiameter * _coefEngine[2], 
                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY),true, horizLocationEng)),

                    // Создание сечения серединной части двигателя 
                    CreateCircle((_infAirliner.Wingspan * 0.5) - (engPos * curEng),
                        - ((_infAirliner.VerticalPositionWing * _coefEngine[0]) + (engPos * curEng) * _coefEngine[1]),
                        _infAirliner.FuselageDiameter * _coefEngine[3], 
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, horizLocationEng + (_infAirliner.LengthOfAircraft * _coefEngine[4]))),
                    
                    // Создание сечения конечной части двигателя 
                    CreateCircle((_infAirliner.Wingspan * 0.5) - (engPos * curEng),
                        - ((_infAirliner.VerticalPositionWing * _coefEngine[0]) + (engPos * curEng) * _coefEngine[1]),
                        _infAirliner.FuselageDiameter * _coefEngine[5],
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, horizLocationEng + (_infAirliner.LengthOfAircraft * _coefEngine[6])))
                };
            
            // Создание списка двигателей и креплений двигателя
            var entityCollection = new List<ksEntity>
                {
                    // Выдавливание двигателя по сечениям
                    ExtrudeLoft(_entityCollection),
                    // Вырезание углубления в двигателе
                    CutExtruded(_entityCollection[0], false, 1, 1),
                    // Вырезание углубления в двигателе
                    CutExtruded(_entityCollection[2], true, 0, 1),
                    // Создание креплений двигателей
                    CreateMountEngine(curEng, engPos, horizLocationEng)
                };
            return entityCollection;
        }
        
        /// <summary>
        /// Крепление двигателя к крылу
        /// </summary>
        /// <returns></returns>
        private ksEntity CreateMountEngine(int curEng, double engPos, double horizLocationEng)
        {
            // Создание списка креплений 
            var entityCollection = new List<ksEntity>
                {
                    // Создание сечения верхней части крепления
                    CreateEllipse((_infAirliner.Wingspan * _coefMountEng[0]) - (engPos * curEng),
                        -horizLocationEng,
                        _infAirliner.FuselageDiameter * _coefMountEng[1],
                        _infAirliner.FuselageDiameter * _coefMountEng[2],
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, ((_infAirliner.VerticalPositionWing * _coefMountEng[3]) + (engPos * curEng) * _coefMountEng[4]) - (_infAirliner.FuselageDiameter * _coefMountEng[5]) )), // зависимость от двигателей зделать 1,25 1,963
                    // Создание сечения нижней части крепления
                    CreateEllipse((_infAirliner.Wingspan * _coefMountEng[0]) - (engPos * curEng),
                        -horizLocationEng - _infAirliner.FuselageDiameter * _coefMountEng[6],
                        _infAirliner.FuselageDiameter * _coefMountEng[7],
                        _infAirliner.FuselageDiameter * _coefMountEng[6],
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, ((_infAirliner.VerticalPositionWing * _coefMountEng[3]) + (engPos * curEng) * _coefMountEng[4]) - _infAirliner.FuselageDiameter * _coefMountEng[8])) // 3,05
                };
            // Выдавливание крепления по сечениям
            return ExtrudeLoft(entityCollection);
        }
        
        /// <summary>
        /// Создание хвостового оперения самолёта
        /// </summary>
        private void CreateTail()
        {
            // Создание списка сечений киля
            _entityCollection = new List<ksEntity>
                {
                    // Создание сечения нижней части киля
                    CreateEllipse(0,
                        -_infAirliner.LengthOfAircraft * _coefTail[0],
                        _infAirliner.FuselageDiameter * _coefTail[1],
                        _infAirliner.LengthOfAircraft * _coefTail[2],
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), true, _infAirliner.FuselageDiameter * _coefTail[3])),

                    // Создание сечения верхней части киля
                    CreateEllipse(0,
                        _infAirliner.LengthOfAircraft * _coefTail[4],
                        _infAirliner.FuselageDiameter * _coefTail[4],
                        _infAirliner.LengthOfAircraft * _coefTail[5],
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), true, _infAirliner.HeightOfKeel))
                };

            // Выдавливание киля по сечениям
            ExtrudeLoft(_entityCollection);

            // Очиска списка
            _entityCollection.Clear();

            // Создание списка сечений для горизонтального стабилизатора
            _entityCollection = new List<ksEntity>
                {
                    // Создание сечения левой части горизонтального оперения 
                    CreateEllipse(-_infAirliner.LengthOfAircraft * _coefTail[6],
                        -_infAirliner.FuselageDiameter * _coefTail[7],
                        _infAirliner.LengthOfAircraft * _coefTail[6],
                        _infAirliner.FuselageDiameter * _coefTail[4],
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, _infAirliner.LengthOfHorizontalStabilizer * _coefTail[8])),

                    // Создание сечения серединной части горизонтального оперения 
                    CreateEllipse(-_infAirliner.LengthOfAircraft * _coefTail[9],
                        -_infAirliner.FuselageDiameter * _coefTail[10],
                        _infAirliner.LengthOfAircraft * _coefTail[11],
                        _infAirliner.FuselageDiameter * _coefTail[12],
                        0,
                        GetPlane(Obj3dType.o3d_planeYOZ)),

                    // Создание сечения правой части горизонтального оперения 
                    CreateEllipse(-_infAirliner.LengthOfAircraft * _coefTail[6],
                        -_infAirliner.FuselageDiameter * _coefTail[7],
                        _infAirliner.LengthOfAircraft * _coefTail[6],
                        _infAirliner.FuselageDiameter * _coefTail[4],
                        0, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.LengthOfHorizontalStabilizer * _coefTail[8]))
                };
            // Выдавливание горизонтального стабилизатора по сечениям
            ExtrudeLoft(_entityCollection);
            _entityCollection.Clear();
        }

        /// <summary>
        /// Создание окон
        /// </summary>
        private void CreateWindows()
        {
            // Вырезание левого лобового окна
            CutExtruded(CreateRectangle(-_infAirliner.FuselageDiameter * _coefWindows[0],
                            _infAirliner.FuselageDiameter * _coefWindows[1],
                            _infAirliner.FuselageDiameter * _coefWindows[2],
                            _infAirliner.FuselageDiameter * _coefWindows[3],
                            0,
                            MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * _coefWindows[4])),
                true, 0, 3);
            // Вырезание правого лобового окна
            CutExtruded(CreateRectangle(_infAirliner.FuselageDiameter * _coefWindows[5], 
                            _infAirliner.FuselageDiameter * _coefWindows[1],
                            _infAirliner.FuselageDiameter * _coefWindows[2],
                            _infAirliner.FuselageDiameter * _coefWindows[3],
                            0,
                            MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * _coefWindows[4])),
                true, 0, 3);

            // Расчёт количестава боковых окон
            var numWindows = Convert.ToInt32(_infAirliner.LengthOfAircraft * _coefWindows[6]) / (_infAirliner.LengthOfAircraft * _coefWindows[7]);
            
            // Вырезание боковых окон и их зеркальное копирование
            CopyBody(CutExtruded(CreateSideWindows(-(_infAirliner.LengthOfAircraft * _coefWindows[8]),
                        -_infAirliner.FuselageDiameter * _coefWindows[9],
                        _infAirliner.LengthOfAircraft * _coefWindows[10],
                        _infAirliner.FuselageDiameter * _coefWindows[11],
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.FuselageDiameter * _coefWindows[12]),
                        numWindows), false, 0, 3),
                        GetPlane(Obj3dType.o3d_planeYOZ));
            
        }

        /// <summary>
        /// Создание боковых окон
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="angle"></param>
        /// <param name="plane"></param>
        /// <param name="numWindows"></param>
        /// <returns></returns>
        private ksEntity CreateSideWindows(double xc, double yc, double a, double b, double angle, ksEntity plane, double numWindows)
        {   
            ksEntity baseSketch = null;
            if (_part != null)
            {
                baseSketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
                if (baseSketch != null)
                {
                    var definitionSketch = (ksSketchDefinition)baseSketch.GetDefinition();
                    if (definitionSketch != null)
                    {
                        var ellipseParam = (ksEllipseParam)_kompasObj.GetParamStruct((short)StructType2DEnum.ko_EllipseParam);
                        for (var i = 1; i <= numWindows; i++)
                        {
                            ellipseParam.Init();
                            ellipseParam.A = a;
                            ellipseParam.B = b;
                            ellipseParam.angle = angle;
                            ellipseParam.style = 1;
                            ellipseParam.xc = xc - (_infAirliner.LengthOfAircraft * 0.0132 * i);
                            ellipseParam.yc = yc;
                            definitionSketch.SetPlane(plane);
                            baseSketch.Create();
                            var sketchEdit = (ksDocument2D) definitionSketch.BeginEdit();
                            sketchEdit.ksEllipse(ellipseParam);
                            definitionSketch.EndEdit();
                        }
                    }
                }
            }
            return baseSketch;
        }
        
        /// <summary>
        /// Создание шасси
        /// </summary>
        private void CreateChassis()
        {
            //Передние колёса
            //Создание переднего левого колеса и его зеркальное копирование 
            CopyBody(CreateWheel(_infAirliner.LengthOfAircraft * _coefChassis[0],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[2],
                        _infAirliner.FuselageDiameter * _coefChassis[3]),
                MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0));

            //Создание сечений переднего вала
            var entityCollection = new List<ksEntity>
                {
                    CreateCircle(-_infAirliner.LengthOfAircraft * _coefChassis[0],
                        _infAirliner.FuselageDiameter * _coefChassis[1], 
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, _infAirliner.FuselageDiameter * _coefChassis[5])),

                    CreateCircle(-_infAirliner.LengthOfAircraft * _coefChassis[0], 
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.FuselageDiameter * _coefChassis[5]))
                };
            // Выдавливание вала по сечениям
            ExtrudeLoft(entityCollection);
            entityCollection.Clear();

            CreateShaftAndRack(0, -_infAirliner.LengthOfAircraft * _coefChassis[0],
                               _infAirliner.FuselageDiameter*_coefChassis[4], Obj3dType.o3d_planeXOZ, false,
                               _infAirliner.FuselageDiameter*_coefChassis[6], _infAirliner.FuselageDiameter*_coefChassis[1]);

            // Задние колёса
            // Создание списка заднего левого шасси для зеркального отображения
            _mirrorCopyEntityCollection = new List<ksEntity>
                {
                    // Создание правого колеса первого ряда
                    CreateWheel(_infAirliner.HorizontalPositionWing * _coefChassis[13],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[2],
                        _infAirliner.FuselageDiameter * _coefChassis[7]),
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),

                    // Создание левого колеса первого ряда
                    CreateWheel(_infAirliner.HorizontalPositionWing * _coefChassis[13],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[2],
                        _infAirliner.FuselageDiameter * _coefChassis[8]),
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),

                    // Создание правого колеса второго ряда
                    CreateWheel(_infAirliner.HorizontalPositionWing * _coefChassis[9],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[2],
                        _infAirliner.FuselageDiameter * _coefChassis[7]),
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),

                    // Создание  левого колеса второго ряда
                    CreateWheel(_infAirliner.HorizontalPositionWing * _coefChassis[9],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[2],
                        _infAirliner.FuselageDiameter * _coefChassis[8]),
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),

                    // Создание правого колеса третьего ряда
                    CreateWheel(_infAirliner.HorizontalPositionWing * _coefChassis[10], 
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[2],
                        _infAirliner.FuselageDiameter * _coefChassis[7]),
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),

                    // Создание левого колеса третьего ряда
                    CreateWheel(_infAirliner.HorizontalPositionWing * _coefChassis[10],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[2],
                        _infAirliner.FuselageDiameter * _coefChassis[8]),
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),
                
                    // Создание вала первого ряда колёс
                    CreateShaftAndRack(_infAirliner.HorizontalPositionWing * _coefChassis[13],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        Obj3dType.o3d_planeYOZ,
                        true,
                        _infAirliner.FuselageDiameter * _coefChassis[11],
                        _infAirliner.FuselageDiameter * _coefChassis[12]),
                    // Создание вала второго ряда колёс
                    CreateShaftAndRack(_infAirliner.HorizontalPositionWing * _coefChassis[9],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        Obj3dType.o3d_planeYOZ,
                        true,
                        _infAirliner.FuselageDiameter * _coefChassis[11],
                        _infAirliner.FuselageDiameter * _coefChassis[12]),
                    // Создание вала третьего ряда колёс
                    CreateShaftAndRack(_infAirliner.HorizontalPositionWing * _coefChassis[10],
                        _infAirliner.FuselageDiameter * _coefChassis[1],
                        _infAirliner.FuselageDiameter * _coefChassis[4], 
                        Obj3dType.o3d_planeYOZ, 
                        true,
                        _infAirliner.FuselageDiameter * _coefChassis[11],
                        _infAirliner.FuselageDiameter * _coefChassis[12]),

                    // Создание первой стойки
                    CreateShaftAndRack(_infAirliner.FuselageDiameter * _coefChassis[13],
                        -_infAirliner.HorizontalPositionWing * _coefChassis[15],
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        Obj3dType.o3d_planeXOZ,
                        false,
                        _infAirliner.FuselageDiameter * _coefChassis[6],
                        _infAirliner.FuselageDiameter * _coefChassis[1]),

                    // Создание второй стойки
                    CreateShaftAndRack(_infAirliner.FuselageDiameter * _coefChassis[13],
                        -_infAirliner.HorizontalPositionWing * _coefChassis[14],
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        Obj3dType.o3d_planeXOZ,
                        false,
                        _infAirliner.FuselageDiameter * _coefChassis[6],
                        _infAirliner.FuselageDiameter * _coefChassis[1]),

                    // Создание ребра жёсткости для передней стойки
                    CreateShaftAndRack(_infAirliner.FuselageDiameter * _coefChassis[13],
                        -_infAirliner.HorizontalPositionWing * _coefChassis[15],
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        Obj3dType.o3d_planeXOZ,
                        false,
                        _infAirliner.FuselageDiameter * _coefChassis[6],
                        _infAirliner.FuselageDiameter * _coefChassis[1]),

                    // Создание ребра жёсткости для задней стойки
                    CreateShaftAndRack(_infAirliner.FuselageDiameter * _coefChassis[13],
                        -_infAirliner.HorizontalPositionWing * _coefChassis[14], 
                        _infAirliner.FuselageDiameter * _coefChassis[4],
                        Obj3dType.o3d_planeXOZ,
                        false,
                        _infAirliner.FuselageDiameter * _coefChassis[6],
                        _infAirliner.FuselageDiameter * _coefChassis[1]),
                    
                    // Создание складывающего подкоса передней стойки
                    CreateFoldingStrut(_infAirliner.FuselageDiameter * _coefChassis[13], 
                        _infAirliner.HorizontalPositionWing * _coefChassis[15], 
                        _infAirliner.FuselageDiameter * _coefChassis[4]),

                    // Создание складывающего подкоса задней стойки
                    CreateFoldingStrut(_infAirliner.FuselageDiameter * _coefChassis[13],
                        _infAirliner.HorizontalPositionWing * _coefChassis[14], 
                        _infAirliner.FuselageDiameter * _coefChassis[4])
                };

            // Создание сечений оперечного вала
            entityCollection.Add(CreateCircle(-_infAirliner.FuselageDiameter * _coefChassis[13],
                                    -_infAirliner.FuselageDiameter * _coefChassis[1],
                                    _infAirliner.FuselageDiameter * _coefChassis[4],
                                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.HorizontalPositionWing * _coefChassis[16])));

            entityCollection.Add(CreateCircle(-_infAirliner.FuselageDiameter * _coefChassis[13],
                                    -_infAirliner.FuselageDiameter * _coefChassis[1],
                                    _infAirliner.FuselageDiameter * _coefChassis[4],
                                    MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.HorizontalPositionWing * _coefChassis[17])));
            // Выдавливание поперечного вала по сечениям и добавление в список для зеркалирования
            _mirrorCopyEntityCollection.Add(ExtrudeLoft(entityCollection));
            entityCollection.Clear();

            // Зеркалирование шасси
            CopyBody(_mirrorCopyEntityCollection, GetPlane(Obj3dType.o3d_planeYOZ));
        }

        /// <summary>
        /// Создание колеса
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="r"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        private ksEntity CreateWheel(double xc, double yc, double r, double move)
        {
            // Создание списка сечений для создания колеса
            var entityCollection = new List<ksEntity>
                {
                    // Создание сечения левой части колеса
                    CreateCircle(-xc, 
                        yc, 
                        r * 0.75, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move)),
                    // Создание сечения левой серединной части колеса
                    CreateCircle(-xc,
                        yc, 
                        r, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move - (_infAirliner.FuselageDiameter * 0.133) * 0.3)),
                    // Создание сечения правой серединной части колеса
                    CreateCircle(-xc,
                        yc,
                        r, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move - (_infAirliner.FuselageDiameter * 0.133) * 0.5)),
                    // Создание сечения левой серединной части колеса
                    CreateCircle(-xc,
                        yc,
                        r * 0.75,
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move - (_infAirliner.FuselageDiameter * 0.133) * 0.8))
                };
            // Выдавливание колеса по сечениям
            return ExtrudeLoft(entityCollection);
        }

        /// <summary>
        /// Создание деталей шасси
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="r"></param>
        /// <param name="plane"></param>
        /// <param name="direction"></param>
        /// <param name="beginDetails"></param>
        /// <param name="endDetails"></param>
        /// <returns></returns>
        private ksEntity CreateShaftAndRack(double xc, double yc, double r, Obj3dType plane, bool direction, double beginDetails, double endDetails)
        {
            // Создание списка сечений для деталей шасси
            var entityCollection = new List<ksEntity>
                {
                    // Создание сечения начала деталей шасси
                    CreateCircle(-xc,
                        yc,
                        r, 
                        MovePlane(GetPlane(plane), direction, beginDetails)),
                    // Создание сечения конца деталей шасси
                    CreateCircle(-xc,
                        yc, 
                        r,
                        MovePlane(GetPlane(plane), direction, endDetails))
                };
            // Выдавливание детали шасси по сечениям
            return ExtrudeLoft(entityCollection);
        }

        /// <summary>
        /// Создание складывающего подкоса
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private ksEntity CreateFoldingStrut(double xc, double yc, double r)
        {
            // Создание списка сечений для складывающего подкоса
            var entityCollection = new List<ksEntity>
                {
                    // Создание сечения начала складывающего подкоса
                    CreateCircle(-xc + _infAirliner.LengthOfAircraft * 0.013, 
                        -yc,
                        r, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, _infAirliner.FuselageDiameter * 0.17)),

                    // Создание сечения конца складывающего подкоса
                    CreateCircle(-xc,
                        -yc, 
                        r, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false,  _infAirliner.FuselageDiameter * 0.45))
                };
            // Выдавливание складывающего подкоса по сечениям
            return ExtrudeLoft(entityCollection);
        }

        /// <summary>
        /// Создание окружности
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        /// <param name="plane"></param>
        private ksEntity CreateCircle(double x, double y, double radius, ksEntity plane)
        {
            // Создание объекта ksEntity
            ksEntity baseSketch = null;
            if (_part != null)
            {
                // Присваивание к объекту новый эскиза
                baseSketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
                if (baseSketch != null)
                {
                    // Получение интерфейса ksSketchDefinition
                    var definitionSketch = (ksSketchDefinition)baseSketch.GetDefinition();
                    if (definitionSketch != null)
                    {
                        // Задание плоскости
                        definitionSketch.SetPlane(plane);
                        // Создание эскиза
                        baseSketch.Create();
                        // Начало операции
                        var sketchEdit = (ksDocument2D)definitionSketch.BeginEdit();
                        // Создание круга
                        sketchEdit.ksCircle(x, y, radius, 1);
                        // Конец операции
                        definitionSketch.EndEdit();
                    }
                }
            }
            // Возврат объекта
            return baseSketch;
        }

        /// <summary>
        /// Создание окружности
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="plane"></param>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        private ksEntity CreateEllipse(double xc, double yc, double a, double b, double angle, ksEntity plane)
        {
            ksEntity baseSketch = null;
            if (_part != null)
            {
                baseSketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
                if (baseSketch != null)
                {
                    var definitionSketch = (ksSketchDefinition)baseSketch.GetDefinition();
                    if (definitionSketch != null)
                    {
                        var ellipseParam = (ksEllipseParam)_kompasObj.GetParamStruct((short)StructType2DEnum.ko_EllipseParam);
                        ellipseParam.Init();
                        ellipseParam.A = a;
                        ellipseParam.B = b;
                        ellipseParam.angle = angle;
                        ellipseParam.style = 1;
                        ellipseParam.xc = xc;
                        ellipseParam.yc = yc;
                        definitionSketch.SetPlane(plane);
                        baseSketch.Create();
                        var sketchEdit = (ksDocument2D)definitionSketch.BeginEdit();
                        sketchEdit.ksEllipse(ellipseParam);
                        definitionSketch.EndEdit();
                    }
                }
            }
            return baseSketch;
        }

        /// <summary>
        /// Создание прямоугольника
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="height"></param>
        /// <param name="width"></param>
        /// <param name="angle"></param>
        /// <param name="plane"></param>
        /// <returns></returns>
        private ksEntity CreateRectangle(double x, double y, double height, double width, double angle, ksEntity plane)
        {
            ksEntity baseSketch = null;
            if (_part != null)
            {
                baseSketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
                if (baseSketch != null)
                {
                    var definitionSketch = (ksSketchDefinition)baseSketch.GetDefinition();
                    if (definitionSketch != null)
                    {
                        var rectangleParam = (ksRectangleParam)_kompasObj.GetParamStruct((short)StructType2DEnum.ko_RectangleParam);
                        rectangleParam.Init();
                        rectangleParam.x = x;
                        rectangleParam.y = y;
                        rectangleParam.ang = angle;
                        rectangleParam.style = 1;
                        rectangleParam.height = height;
                        rectangleParam.width = width;
                        definitionSketch.SetPlane(plane);
                        baseSketch.Create();
                        var sketchEdit = (ksDocument2D)definitionSketch.BeginEdit();
                        sketchEdit.ksRectangle(rectangleParam);
                        definitionSketch.EndEdit();
                    }
                }
            }
            return baseSketch;
        }

        /// <summary>
        /// Тип плоскости
        /// </summary>
        /// <param name="typePlane"></param>
        /// <returns></returns>
        private ksEntity GetPlane(Obj3dType typePlane)
        {
            var plane = (ksEntity)_part.GetDefaultEntity((short)typePlane);
            return plane;
        }

        /// <summary>
        /// Смещение плоскости
        /// </summary>
        /// <param name="entityPlane"></param>
        /// <param name="direction"></param>
        /// <param name="move"></param>
        /// <returns></returns>
        private ksEntity MovePlane(ksEntity entityPlane, bool direction, double move)
        {
            var entityOffsetPlane = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_planeOffset);
            var entityPlaneOffsetDefinition = (ksPlaneOffsetDefinition)entityOffsetPlane.GetDefinition();

            entityPlaneOffsetDefinition.direction = direction;
            entityPlaneOffsetDefinition.offset = move;
            entityPlaneOffsetDefinition.SetPlane(entityPlane);
            entityOffsetPlane.Create();
            
            return entityOffsetPlane;
        }
        
        /// <summary>
        /// Выдавливание по сечениям
        /// </summary>
        private ksEntity ExtrudeLoft(List<ksEntity> entityCollection)
        {
            var entityLoft = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_baseLoft);
            var sketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
            var definitionSketch = (ksSketchDefinition)entityCollection[0].GetDefinition();

            if (entityLoft != null)
            {
                var extrusionDef = (ksBaseLoftDefinition)entityLoft.GetDefinition();
                var collectionSketch = (ksEntityCollection)extrusionDef.Sketchs();
                foreach (var entity in entityCollection)
                {
                    collectionSketch.Add(entity);
                }
                entityLoft.Create();
                entityLoft.Update();
            }
            definitionSketch.EndEdit();
            sketch.Update();
            return entityLoft;
        }

        /// <summary>
        /// Вырезание выдавливанием
        /// </summary>
        /// <param name="entityCut"></param>
        /// <param name="side"></param>
        /// <param name="typeDirection"></param>
        /// <param name="depth"></param>
        private ksEntity CutExtruded(ksEntity entityCut, bool side, int typeDirection, double depth)
        {
            var entityCutExtr = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_cutExtrusion);
            var cutExtrDefinition = (ksCutExtrusionDefinition)entityCutExtr.GetDefinition();
            cutExtrDefinition.cut = true;
            cutExtrDefinition.directionType = (short)typeDirection;
            cutExtrDefinition.SetSideParam(side, 0, depth);
            cutExtrDefinition.SetSketch(entityCut);
            entityCutExtr.Create();
            return entityCutExtr;
        }
        
        /// <summary>
        /// Сопирование тела
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="plane"></param>
        private void CopyBody(ksEntity entity, ksEntity plane)
        {
            var entityMirrorOperation = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_mirrorOperation);            var mirrorCopyDefinition = (ksMirrorCopyDefinition)entityMirrorOperation.GetDefinition();
            mirrorCopyDefinition.SetPlane(plane);
            var entityCollection = (ksEntityCollection) mirrorCopyDefinition.GetOperationArray();
            entityCollection.Clear();
            entityCollection.Add(entity);
            entityMirrorOperation.Create();
        }

        /// <summary>
        /// Зеркальное копирование тела относительно плоскости
        /// </summary>
        /// <param name="entityCollection"></param>
        /// <param name="plane"></param>
        private void CopyBody(List<ksEntity> entityCollection, ksEntity plane)
        {
            var entityMirrorOperation = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_mirrorOperation);
            var mirrorCopyDefinition = (ksMirrorCopyDefinition)entityMirrorOperation.GetDefinition();
            mirrorCopyDefinition.SetPlane(plane);
            var copyEntityCollection = (ksEntityCollection)mirrorCopyDefinition.GetOperationArray();
            copyEntityCollection.Clear();
            foreach (var entity in entityCollection)
            {
                copyEntityCollection.Add(entity);
            }
            entityMirrorOperation.Create();
        }

        /// <summary>
        /// Скругление рёбер
        /// </summary>
        private void Rounding(double radius, int numEdge)
        {
            var entityFillet = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_fillet);
            var filletDefinition = (ksFilletDefinition)entityFillet.GetDefinition();
            filletDefinition.radius = radius;
            filletDefinition.tangent = false;
            var entityCollectionPart = (ksEntityCollection)_part.EntityCollection((short)Obj3dType.o3d_edge);
            var entityCollectionFillet = (ksEntityCollection)filletDefinition.array();
            entityCollectionFillet.Clear();
            entityCollectionFillet.Add(entityCollectionPart.GetByIndex(numEdge));
            entityFillet.Create();
        }
    }
}
