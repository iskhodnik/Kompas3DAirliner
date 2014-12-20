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
                CreateCircle(0, _infAirliner.FuselageDiameter * 0.25, _infAirliner.FuselageDiameter * 0.0625, GetPlane(Obj3dType.o3d_planeXOY)),
                CreateCircle(0, 0, _infAirliner.FuselageDiameter  * 0.5, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * 0.33)),
                CreateCircle(0, 0, _infAirliner.FuselageDiameter  * 0.5, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * 0.78)),
                CreateCircle(0, 0, _infAirliner.FuselageDiameter * 0.458, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * 0.85)),
                CreateCircle(0, 0, _infAirliner.FuselageDiameter * 0.308, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * 0.93)),
                CreateCircle(0, 0, _infAirliner.FuselageDiameter * 0.0625, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft))
            };
            ExtrudeLoft(_entityCollection);
            Rounding(_infAirliner.FuselageDiameter * 0.0625, 0);
            Rounding(_infAirliner.FuselageDiameter * 0.0625, 0);
            _entityCollection.Clear();

            CreateWindows();
        }

        /// <summary>
        /// Создание крыльев самолёта
        /// </summary>
        private void CreateWing()
        {
            _entityCollection = new List<ksEntity>
                {
                    CreateEllipse(-(_infAirliner.HorizontalPositionWing), 
                        _infAirliner.FuselageDiameter * 0.25, 
                        _infAirliner.FuselageDiameter * 0.9375,
                        _infAirliner.FuselageDiameter * 0.15625,
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, 0)),

                    CreateEllipse(-(_infAirliner.HorizontalPositionWing - (_infAirliner.Wingspan * 0.18 * Math.Tan(_infAirliner.SweepbackAngle * Math.PI / 180))),
                        _infAirliner.FuselageDiameter * 0.125,
                        _infAirliner.FuselageDiameter * 0.515625,
                        _infAirliner.FuselageDiameter * 0.1125,
                        0, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.Wingspan * 0.18)),

                    CreateEllipse(-(_infAirliner.HorizontalPositionWing - (_infAirliner.Wingspan * 0.5 * Math.Tan(_infAirliner.SweepbackAngle * Math.PI / 180))), 
                        (_infAirliner.FuselageDiameter * 0.25) - (_infAirliner.FuselageDiameter * 0.345),
                        _infAirliner.FuselageDiameter * 0.20625, _infAirliner.FuselageDiameter * 0.01875,
                        0, 
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.Wingspan * 0.5))
                };
            var entityWing = ExtrudeLoft(_entityCollection);
            CopyBody(entityWing, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0));
            _entityCollection.Clear();
        }
        
        /// <summary>
        /// Создание двигателей
        /// </summary>
        private void CreateEngines()
        {
            _mirrorCopyEntityCollection = new List<ksEntity>();
            for (var i = 1; i <= (int)_infAirliner.TypeQuantityOfEngine * 0.5; i++) 
            {
                foreach (var entity in CreateEngine(i))
                {
                    _mirrorCopyEntityCollection.Add(entity);
                }
            }
            CopyBody(_mirrorCopyEntityCollection, GetPlane(Obj3dType.o3d_planeYOZ));
        }

        /// <summary>
        /// Создание одного двигателя
        /// </summary>
        private List<ksEntity> CreateEngine(int curEng)
        {
            var engPos = ((_infAirliner.Wingspan - _infAirliner.FuselageDiameter) * 0.5) / (((int)_infAirliner.TypeQuantityOfEngine / 2) + 1);
            
            var horizLocationEng = (_infAirliner.HorizontalPositionWing - ((_infAirliner.Wingspan * 0.5 - (engPos * curEng)) * Math.Tan(_infAirliner.SweepbackAngle * Math.PI / 180)));
            
            _entityCollection = new List<ksEntity>
                {
                    CreateCircle((_infAirliner.Wingspan * 0.5) - (engPos * curEng), - ((_infAirliner.VerticalPositionWing * 0.366) + (engPos * curEng) * 0.12), _infAirliner.FuselageDiameter * 0.0917, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, horizLocationEng)),
                    CreateCircle((_infAirliner.Wingspan * 0.5) - (engPos * curEng), - ((_infAirliner.VerticalPositionWing * 0.366) + (engPos * curEng) * 0.12), _infAirliner.FuselageDiameter * 0.15, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, horizLocationEng + (_infAirliner.LengthOfAircraft * 0.093))),
                    CreateCircle((_infAirliner.Wingspan * 0.5) - (engPos * curEng), - ((_infAirliner.VerticalPositionWing * 0.366) + (engPos * curEng) * 0.12), _infAirliner.FuselageDiameter * 0.115, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, horizLocationEng + (_infAirliner.LengthOfAircraft * 0.123)))
                };
            
            var entityCollection = new List<ksEntity>
                {
                    ExtrudeLoft(_entityCollection),
                    CutExtruded(_entityCollection[0], false, 1, 1),
                    CutExtruded(_entityCollection[2], true, 0, 1),
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

            var entityCollection = new List<ksEntity>
                {
                    CreateEllipse((_infAirliner.Wingspan * 0.5) - (engPos * curEng), -horizLocationEng,
                        _infAirliner.FuselageDiameter * 0.016, _infAirliner.FuselageDiameter * 0.266, 0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, ((_infAirliner.VerticalPositionWing * 0.366) + (engPos * curEng) * 0.12) - (_infAirliner.FuselageDiameter * 0.3) )), // зависимость от двигателей зделать 1,25 1,963
                    CreateEllipse((_infAirliner.Wingspan * 0.5) - (engPos * curEng), -horizLocationEng - _infAirliner.FuselageDiameter * 0.416,
                        _infAirliner.FuselageDiameter * 0.025, _infAirliner.FuselageDiameter * 0.416, 0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, ((_infAirliner.VerticalPositionWing * 0.366) + (engPos * curEng) * 0.12) - _infAirliner.FuselageDiameter * 0.0917)) // 3,05
                };
            return ExtrudeLoft(entityCollection);
        }

        /// <summary>
        /// Создание хвостового оперения самолёта
        /// </summary>
        private void CreateTail()
        {
            _entityCollection = new List<ksEntity>
                {
                    CreateEllipse(0, -_infAirliner.LengthOfAircraft * 0.106, _infAirliner.FuselageDiameter * 0.069, _infAirliner.LengthOfAircraft * 0.081, 0, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), true, _infAirliner.FuselageDiameter * 0.25)),
                    CreateEllipse(0, _infAirliner.LengthOfAircraft * 0.023, _infAirliner.FuselageDiameter * 0.023, _infAirliner.LengthOfAircraft * 0.029, 0, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), true, _infAirliner.HeightOfKeel))
                };
            ExtrudeLoft(_entityCollection);
            _entityCollection.Clear();
            _entityCollection = new List<ksEntity>
                {
                    CreateEllipse(-_infAirliner.LengthOfAircraft * 0.024, -_infAirliner.FuselageDiameter * 0.289, _infAirliner.LengthOfAircraft * 0.024, _infAirliner.FuselageDiameter * 0.023, 0, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, _infAirliner.LengthOfHorizontalStabilizer * 0.5)),
                    CreateEllipse(-_infAirliner.LengthOfAircraft * 0.081, -_infAirliner.FuselageDiameter * 0.255, _infAirliner.LengthOfAircraft * 0.061, _infAirliner.FuselageDiameter * 0.04, 0, GetPlane(Obj3dType.o3d_planeYOZ)),
                    CreateEllipse(-_infAirliner.LengthOfAircraft * 0.024, -_infAirliner.FuselageDiameter * 0.289, _infAirliner.LengthOfAircraft * 0.024, _infAirliner.FuselageDiameter * 0.023, 0, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.LengthOfHorizontalStabilizer * 0.5))
                };
            ExtrudeLoft(_entityCollection);
            _entityCollection.Clear();
        }

        /// <summary>
        /// Создание окон
        /// </summary>
        private void CreateWindows()
        {
            CutExtruded(CreateRectangle(_infAirliner.FuselageDiameter * -0.2625, _infAirliner.FuselageDiameter * 0.1875,
                _infAirliner.FuselageDiameter * 0.125, _infAirliner.FuselageDiameter * 0.25, 0,
                MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * 0.95)), true, 0, 3);

            CutExtruded(CreateRectangle(_infAirliner.FuselageDiameter * 0.0125, _infAirliner.FuselageDiameter * 0.1875,
                _infAirliner.FuselageDiameter * 0.125, _infAirliner.FuselageDiameter * 0.25, 0,
                MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.LengthOfAircraft * 0.95)), true, 0, 3);

            var numWindows = Convert.ToInt32(_infAirliner.LengthOfAircraft * 0.53) / (_infAirliner.LengthOfAircraft * 0.01);


            CopyBody(CutExtruded(CreateSideWindows(-(_infAirliner.LengthOfAircraft * 0.18),
                        -_infAirliner.FuselageDiameter * 0.03,
                        _infAirliner.LengthOfAircraft * 0.0033,
                        _infAirliner.FuselageDiameter * 0.05,
                        0,
                        MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.FuselageDiameter * 0.416),
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
            CopyBody(CreateWheel(_infAirliner.LengthOfAircraft * 0.86, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.108, _infAirliner.FuselageDiameter * 0.1385), MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0));
            var entityCollection = new List<ksEntity>
                {
                    CreateCircle(-_infAirliner.LengthOfAircraft * 0.86, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.017, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, _infAirliner.FuselageDiameter * 0.033)),
                    CreateCircle(-_infAirliner.LengthOfAircraft * 0.86, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.017, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), false, _infAirliner.FuselageDiameter * 0.033))
                };
            ExtrudeLoft(entityCollection);
            entityCollection.Clear();

            CreateRack(0, _infAirliner.LengthOfAircraft * 0.86, _infAirliner.FuselageDiameter * 0.017);

            //Задние колёса
            _mirrorCopyEntityCollection = new List<ksEntity>
                {
                    CreateWheel(_infAirliner.HorizontalPositionWing * 0.869, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.108, _infAirliner.FuselageDiameter * 1.013), MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),
                    CreateWheel(_infAirliner.HorizontalPositionWing * 0.869, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.108, _infAirliner.FuselageDiameter * 0.846), MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),
                    CreateWheel(_infAirliner.HorizontalPositionWing * 0.915, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.108, _infAirliner.FuselageDiameter * 1.0125), MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),
                    CreateWheel(_infAirliner.HorizontalPositionWing * 0.915, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.108, _infAirliner.FuselageDiameter * 0.846), MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),
                    CreateWheel(_infAirliner.HorizontalPositionWing * 0.961, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.108, _infAirliner.FuselageDiameter * 1.0125), MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),
                    CreateWheel(_infAirliner.HorizontalPositionWing * 0.961, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.108, _infAirliner.FuselageDiameter * 0.846), MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, 0),
                
                    CreateShaft(_infAirliner.HorizontalPositionWing * 0.869, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.017, _infAirliner.FuselageDiameter * 0.946),
                    CreateShaft(_infAirliner.HorizontalPositionWing * 0.915, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.017, _infAirliner.FuselageDiameter * 0.946),
                    CreateShaft(_infAirliner.HorizontalPositionWing * 0.961, _infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.017, _infAirliner.FuselageDiameter * 0.946),

                    CreateRack(_infAirliner.FuselageDiameter * 0.866, _infAirliner.HorizontalPositionWing * 0.902, _infAirliner.FuselageDiameter * 0.017),
                    CreateRack(_infAirliner.FuselageDiameter * 0.866, _infAirliner.HorizontalPositionWing * 0.947, _infAirliner.FuselageDiameter * 0.017),

                    CreateStiffeningPlate(_infAirliner.FuselageDiameter * 0.866, _infAirliner.HorizontalPositionWing * 0.902, _infAirliner.FuselageDiameter * 0.017),
                    CreateStiffeningPlate(_infAirliner.FuselageDiameter * 0.866, _infAirliner.HorizontalPositionWing * 0.947, _infAirliner.FuselageDiameter * 0.017),

                    CreateFoldingStrut(_infAirliner.FuselageDiameter * 0.866, _infAirliner.HorizontalPositionWing * 0.902, _infAirliner.FuselageDiameter * 0.017),
                    CreateFoldingStrut(_infAirliner.FuselageDiameter * 0.866, _infAirliner.HorizontalPositionWing * 0.947, _infAirliner.FuselageDiameter * 0.017)
                };

            //поперечный вал
            entityCollection.Add(CreateCircle(-_infAirliner.FuselageDiameter * 0.866, -_infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.017, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.HorizontalPositionWing * 0.879)));
            entityCollection.Add(CreateCircle(-_infAirliner.FuselageDiameter * 0.866, -_infAirliner.FuselageDiameter * 0.69, _infAirliner.FuselageDiameter * 0.017, MovePlane(GetPlane(Obj3dType.o3d_planeXOY), true, _infAirliner.HorizontalPositionWing * 0.971)));
            _mirrorCopyEntityCollection.Add(ExtrudeLoft(entityCollection));
            entityCollection.Clear();

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
            var entityCollection = new List<ksEntity>
                {
                    CreateCircle(-xc, yc, r * 0.75, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move)),
                    CreateCircle(-xc, yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move - (_infAirliner.FuselageDiameter * 0.133) * 0.3)),
                    CreateCircle(-xc, yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move - (_infAirliner.FuselageDiameter * 0.133) * 0.5)),
                    CreateCircle(-xc, yc, r * 0.75, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move - (_infAirliner.FuselageDiameter * 0.133) * 0.8))
                };
            return ExtrudeLoft(entityCollection);
        }

        /// <summary>
        /// Создание вала
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="r"></param>
        /// <param name="move"></param>
        private ksEntity CreateShaft(double xc, double yc, double r, double move)
        {
            //Extrusion(CreateCircle(-xc, yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move)), true, 0, _infAirliner.FuselageDiameter * 0.166); /// 1false 2true
            var entityCollection = new List<ksEntity>
                {
                    CreateCircle(-xc, yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move - _infAirliner.FuselageDiameter * 0.14)),
                    CreateCircle(-xc, yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeYOZ), true, move))
                };
            return ExtrudeLoft(entityCollection);
        }

        /// <summary>
        /// Создание стойки
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="r"></param>
        private ksEntity CreateRack(double xc, double yc, double r)
        {
            var entityCollection = new List<ksEntity>
                {
                    CreateCircle(-xc, -yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, _infAirliner.FuselageDiameter * 0.24)),
                    CreateCircle(-xc, -yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, _infAirliner.FuselageDiameter * 0.69))
                };
            return ExtrudeLoft(entityCollection);
        }

        /// <summary>
        /// Создание ребра жесткости
        /// </summary>
        /// <param name="xc"></param>
        /// <param name="yc"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private ksEntity CreateStiffeningPlate(double xc, double yc, double r)
        {
            var entityCollection = new List<ksEntity>
                {
                    CreateCircle(-xc, -yc - _infAirliner.LengthOfAircraft * 0.013, r, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, _infAirliner.FuselageDiameter * 0.17)),
                    CreateCircle(-xc, -yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false,  _infAirliner.FuselageDiameter * 0.69))
                };
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
            var entityCollection = new List<ksEntity>
                {
                    CreateCircle(-xc + _infAirliner.LengthOfAircraft * 0.013, -yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false, _infAirliner.FuselageDiameter * 0.17)),
                    CreateCircle(-xc, -yc, r, MovePlane(GetPlane(Obj3dType.o3d_planeXOZ), false,  _infAirliner.FuselageDiameter * 0.45))
                };
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
            ksEntity baseSketch = null;
            if (_part != null)
            {
                baseSketch = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_sketch);
                if (baseSketch != null)
                {
                    var definitionSketch = (ksSketchDefinition)baseSketch.GetDefinition();
                    if (definitionSketch != null)
                    {
                        definitionSketch.SetPlane(plane);
                        baseSketch.Create();
                        var sketchEdit = (ksDocument2D)definitionSketch.BeginEdit();
                        sketchEdit.ksCircle(x, y, radius, 1);
                        definitionSketch.EndEdit();
                    }
                }
            }
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
        /// Операция выдавливание
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="forward"></param>
        /// <param name="typeDirection"></param>
        /// <param name="depth"></param>
        /// <returns></returns>
        private ksEntity Extrusion(ksEntity entity, bool forward, int typeDirection, double depth)
        {
            var entityExtr = (ksEntity)_part.NewEntity((short)Obj3dType.o3d_baseExtrusion);
            var extrDefinition = (ksBaseExtrusionDefinition)entityExtr.GetDefinition();
            extrDefinition.SetSideParam(forward, (short)typeDirection, depth, 0, true);
            extrDefinition.SetSketch(entity);
            entityExtr.Create();
            return entityExtr;
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
            //_baseSketch1 = entityMirrorOperation; /// her
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
