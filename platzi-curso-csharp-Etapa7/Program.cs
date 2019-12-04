using System;
using System.Collections.Generic;
using System.Linq;
using CoreEscuela.App;
using CoreEscuela.Entidades;
using CoreEscuela.Util;
using static System.Console;

namespace CoreEscuela
{
    class Program
    {
        static void Main(string[] args)
        {
            // AppDomain.CurrentDomain.ProcessExit += AccionDelEvento;
            // AppDomain.CurrentDomain.ProcessExit += (o, s) => Printer.Beep(2000, 1000, 1);

            var engine = new EscuelaEngine();
            engine.Inicializar();
            Printer.WriteTitle("BIENVENIDOS A LA ESCUELA");

            var reporteador = new Reporteador(engine.GetDiccionarioObjetos());
            var evalList = reporteador.GetListaEvalucion();
            var asigList = reporteador.GetListaAsignaturas();
            var listaEval = reporteador.GetDicEvaluacionAsign();
            var ListaPromexAsig = reporteador.GetPromedioAlumnoAsig();

            Printer.WriteTitle("Captura de una evaluacion por consola");
            var newEval = new Evaluacion();
            string nombre, notastrig;
            float nota;

            WriteLine("Ingrese el Nombre de la evalucion");
            Printer.PresioneEnter();
            nombre = Console.ReadLine();

            if (string.IsNullOrEmpty(nombre))
            {
                throw new ArgumentException($"El valor del {nameof(nombre)} no puede ser vacio");
            }
            else
            {
                newEval.Nombre = nombre.ToLower();
                WriteLine("El nombre de la evaluacion ha sido ingresado");
            }

            /*////////////////////////////////////////////*/
            WriteLine("Ingrese La NOTA de la evalucion");
            Printer.PresioneEnter();
            notastrig = Console.ReadLine();

            if (string.IsNullOrEmpty(notastrig))
            {
                throw new ArgumentException($"El valor del {nameof(notastrig)} no puede ser vacio");
            }
            else
            {
                try
                {
                    newEval.Nota = float.Parse(notastrig);
                    if (newEval.Nota < 0 || newEval.Nota > 5)
                    {
                        throw new ArgumentOutOfRangeException("La nota debe de estar entre 0 y 5");
                    }
                    WriteLine("La nota de la evaluacion ha sido ingresado");
                }
                catch (ArgumentOutOfRangeException ex)
                {
                    Printer.WriteTitle(ex.Message);
                }
                catch (Exception ex)
                {
                    Printer.WriteTitle($"{ex.Message} El Valor de la nota no es un numero valido");
                }
                finally
                {
                    Printer.ExitProgram();
                    Printer.Beep(2500,500,3);
                }
            }
        }

        private static void AccionDelEvento(object sender, EventArgs e)
        {
            Printer.WriteTitle("SALIENDO");
            Printer.Beep(3000, 1000, 3);
            Printer.WriteTitle("SALIÓ");
        }

        private static void ImpimirCursosEscuela(Escuela escuela)
        {

            Printer.WriteTitle("Cursos de la Escuela");


            if (escuela?.Cursos != null)
            {
                foreach (var curso in escuela.Cursos)
                {
                    WriteLine($"Nombre {curso.Nombre  }, Id  {curso.UniqueId}");
                }
            }
        }
    }
}
