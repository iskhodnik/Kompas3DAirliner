using System;
using System.Windows;
using Kompas6API5;

namespace Airliner.Classes
{
    public class Manager
    {
        /// <summary>
        /// Создание модели самолёта
        /// </summary>
        private CreatingModel _creadeModel;

        /// <summary>
        /// Интерфейс системы компас
        /// </summary>
        private KompasObject _kompas;

        /// <summary>
        /// Инициализация модели
        /// </summary>
        /// <param name="infAirliner"></param>
        public void InitializeModel(InfAirliner infAirliner)
        {
            if (_kompas != null)
            {
                var doc3D = (ksDocument3D) _kompas.Document3D();
                doc3D.Create(false, false);
                doc3D = (ksDocument3D) _kompas.ActiveDocument3D();
                _creadeModel = new CreatingModel(doc3D, _kompas, infAirliner);
                _creadeModel.CreateModel();
            }
        }

        /// <summary>
        /// Открытие компаса
        /// </summary>
        public void OpenKompas3D()
        {
            if (_kompas == null)
            {
                var type = Type.GetTypeFromProgID("KOMPAS.Application.5");
                _kompas = (KompasObject)Activator.CreateInstance(type);
            }
            try
            {
                if (_kompas != null)
                {
                    _kompas.Visible = true;
                    _kompas.ActivateControllerAPI();
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Ошибка! Компас-3D не открывается!");
                throw new Exception();
                
            }
        }

        /// <summary>
        /// Обнуление компаса
        /// </summary>
        public void KompasReset()
        {
            var test = "";
            _kompas = null;
        }
    }
}
