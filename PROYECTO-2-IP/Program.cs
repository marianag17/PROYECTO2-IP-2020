using System;
using System.IO;
namespace PROYECTO_2_IP
{
    class Program
    {
        string[] lines, barcos;
        int filas, columnas, correcto, fallido, cantidad, turno = 1, espacios, espaciosop,correctoop; //creación de variables 
        string[,] tablero;
        string[,] visible, oculto;
        string op = "";
        static void Main(string[] args) //main
        {
            Console.WriteLine("      ---------------------------------------------   ");
            Console.WriteLine("      |                                           |   ");
            Console.WriteLine("      |               BATALLA NAVAL               |   "); //título
            Console.WriteLine("      |                                           |   ");
            Console.WriteLine("      ---------------------------------------------   ");
            Console.WriteLine("\n");
            bool juego = true; 
            Random rnd = new Random(); //instancia del random de los números
            Console.WriteLine("Ingrese la ruta del archivo"); //se le solicita al usuario la ruta al archivo .txt
            string ruta = Console.ReadLine();
            Program programa = new Program();
            programa.leerArchivo(ruta); //se lee el archivo y se crea el tablero
            programa.crearTableroOponente(); //se crea el tablero del oponente
            try //try catch para manejar errores
            {
                while (juego) //mientras se siga jugando se repetira este bucle
                {
                    Console.Clear(); //se limpia la consola
                    Console.WriteLine("TU TABLERO \n");
                    programa.imprimirTablero(programa.tablero); //se muestran los tableros
                    Console.WriteLine("TABLERO DEL OPONENTE\n");
                    programa.imprimirTablero(programa.visible);
                    Console.WriteLine("Turno " + Convert.ToString(programa.turno) + "\n"); //se muestra el turno
                    Console.WriteLine("Su turno, ingrese coordenadas: "); //se ingresan las coordenadas del usuario
                    programa.op = Console.ReadLine();
                    if (programa.op.Equals("x") || programa.op.Equals("X"))
                    {
                        break;
                    }
                    else
                    {
                        programa.atacar(programa.op); //se ataca al oponente 
                        programa.aleatorio(rnd); //se hace el turno del oponente
                        programa.turno++; //se aumenta el número de turno
                        juego = programa.verSiSigue();
                    }//se llama la función para ver si el juego continua 
                }
                Console.WriteLine("FIN DEL JUEGO \n");
                Console.WriteLine("Total de turnos: " + programa.turno + "\n");
                Console.WriteLine("Aciertos: \n");
                Console.WriteLine("Jugador " + programa.correcto + "/" + programa.espaciosop + "\n Oponente " + programa.correctoop + "/" + programa.espacios + "\n");
                Console.ReadKey();
            } catch
            {
                Console.WriteLine("Error de entrada");
            }
        }
        public void leerArchivo (string ruta)
        {
            try
            {
                lines = File.ReadAllLines(ruta); //pone cada línea del archivo en un arreglo
                string[] filascol; //en un arreglo se pone el número de filas y de columnas con un split
                filascol = lines[0].Split(',');
                filas = Convert.ToInt32(filascol[0]); //se leen las filas
                columnas = Convert.ToInt32(filascol[1]); //se leen las columnas
                tablero = new string[filas+1, columnas+1]; //se crea el tablero con las dimensiones
                if (Convert.ToInt32(lines[1]) < 12) //se valida si la cantidad de barcos es menor a 12
                {
                    cantidad = Convert.ToInt32(lines[1]); //se establece la cantidad de barcos
                }
                else
                {
                    Console.WriteLine("Cantidad de barcos debe ser menor a 12"); 
                    Console.ReadKey();
                }
                if (filas>15||columnas>15||filas<5||columnas<5) //se valida que las filas y las columnas sean mayor a 5 y menor a 15
                {
                    Console.WriteLine("Cantidad de columnas y filas debe ser mayor a 5 y menor de 15");
                    Console.ReadKey();
                }
                barcos = subArreglo(lines, 2, lines.Length - 2); //se hace un subarreglo únicamente de las líneas de los barcos
                tablero[0, 0] = "     ";
                for (int i=1; i<=filas;i++) //se llena el tablero con el número de líneas y columnas 
                {
                    tablero[i, 0] = "  " + Convert.ToString(i)+ "  ";
                   
                }
                for (int i = 1; i <= columnas; i++) //se llena el tablero con el número de líneas y columnas 
                {
                    tablero[0, i] = "  " + Convert.ToString(i) + "  ";

                }
                foreach (var item in barcos) //foreach para poner los barcos en el tablero
                {
                    string[] condiciones = item.Split(','); //se pone cada condicion en un arreglo [fila,columna,largo,posicion]
                    if (condiciones.Length == 4) //se valida que solo sean 4 condiciones
                    {
                        int fila = Convert.ToInt16(condiciones[0]);
                        int col = Convert.ToInt16(condiciones[1]);
                        int largo = Convert.ToInt16(condiciones[2]);
                        if (largo>3) //se valida que el barco no ocupe más de 3 espacios
                        {
                            Console.WriteLine("Error en el archivo (tamaño del barco debe ser de 1, 2 o 3 posiciones)");
                            Console.ReadKey();
                        }
                        string posicion = condiciones[3];
                        if (fila<= filas&& col<=columnas) //se valida que quepa el barco en el tablero
                        {
                            if (posicion.Equals("H")||posicion.Equals("h")) //si es horizontal
                            {
                                if (col+largo<= columnas) //si cabe en el tablero
                                {
                                    for (int i=col;i<col+largo;i++)
                                    {
                                        if (tablero[fila, i] == "barco") //se valida si existe ya un barco
                                        {
                                            Console.WriteLine("Error en el archivo (barcos se traslapan)");
                                            Console.ReadKey();
                                        }
                                        else if (tablero[fila, i] == null) //se pone un barco en el espacio del tablero
                                        {
                                            tablero[fila, i] = "barco";
                                            espacios++;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error en el archivo (posición o largo de barcos)"); //aquí se valida que quepa en el tablero
                                    Console.ReadKey();
                                }
                            }
                            else if (posicion.Equals("V") || posicion.Equals("v")) //si el barco va vertical
                            {
                                if (filas + largo <= filas)  //se valida que quepa
                                {
                                    for (int i= fila;i<largo;i++)
                                    {
                                        if (tablero[i, col] == "barco") //se valida si existe ya un barco
                                        {
                                            Console.WriteLine("Error en el archivo (barcos se traslapan)");
                                            Console.ReadKey();
                                        }
                                        else if (tablero[i,col]==null) //se pone un barco en el espacio del tablero
                                        {
                                            tablero[i, col] = "barco";
                                            espacios++;
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Error en el archivo (posición o largo de barcos)");
                                    Console.ReadKey();
                                }
                            }
                            else
                            {
                                Console.WriteLine("Error en el archivo (formato de barcos)");
                                Console.ReadKey();
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error en el archivo");
                        Console.ReadKey();
                    }
                }
                for (int i = 0; i < filas+1; i++) //se llena el resto con "-"
                {
                    for (int j = 0; j < columnas+1; j++)
                    {
                        if (tablero[i,j]==null)
                        {
                            tablero[i, j] = "  -  ";
                        }
                    }
                }
            }
            catch
            {
                Console.WriteLine("Error en el archivo");
                Console.ReadKey();
            }
        }
        public void crearTableroOponente () //en este método se crea el tablero del oponente
        {
            visible = new string[filas + 1, columnas + 1]; //se crean dos tableros , uno visible y otro oculto
            oculto = new string[filas + 1, columnas + 1];
            visible[0, 0] = "     ";
            oculto[0, 0] = "     ";
            for (int i = 1; i <= filas; i++) //se ponen los números 
            {
                visible[i, 0] = "  " + Convert.ToString(i) + "  ";
                oculto[i, 0] = "  " + Convert.ToString(i) + "  ";
                
            }
            for (int i = 1; i <= columnas; i++) //se ponen los números 
            {
                visible[0, i] = "  " + Convert.ToString(i) + "  ";
                oculto[0, i] = "  " + Convert.ToString(i) + "  ";

            }
           
            for (int i = 1; i < filas+1; i++)
            {
                for (int j = 1; j < columnas+1; j++)
                {
                    visible[i, j] = "  -  "; //se llena el tablero visible de "-"
                }
            }
            foreach (var item in barcos) //foreach de los barcos 
            {
                string[] condiciones = item.Split(',');
                int fila = Convert.ToInt16(condiciones[1]);//se invierten las filas y las columnas 
                int col = Convert.ToInt16(condiciones[0]);
                int largo = Convert.ToInt16(condiciones[2]);
                string posicion="";
                if (condiciones[3].Equals("H")) //se invierte la posición del barco
                {
                    posicion = "V";
                }
                else if (condiciones[3].Equals("V"))
                {
                    posicion = "H";
                }
                else
                {
                    Console.WriteLine("Error en el archivo (formato de barcos)");
                    Console.ReadKey();
                }
                if (col <= columnas && fila <= filas) //se valida que quepa
                {
                    if (posicion.Equals("H"))
                    {
                        if (col + largo < columnas) //se valida que quepa
                        {

                            for (int i = col; i < col + largo; i++)
                            {
                                if (oculto[fila, i] == "barco") //se valida si existe ya un barco
                                {
                                    Console.WriteLine("Error en el archivo (barcos se traslapan)");
                                    Console.ReadKey();
                                }
                                else if (tablero[fila, i] == null) //se pone un barco en el espacio del tablero
                                {
                                    oculto[fila, i] = "barco";
                                    espaciosop++;
                                }
                            }
                        }
                    }
                    else if (posicion.Equals("V") || posicion.Equals("v"))
                    {
                        if (fila + largo <= filas)
                        {
                            for (int i = fila; i < fila+largo; i++)
                            {
                                if (oculto[i, col] == "barco") //se valida si existe ya un barco
                                {
                                    Console.WriteLine("Error en el archivo (barcos se traslapan)");
                                    Console.ReadKey();
                                }
                                else if (oculto[i, col] == null) //se pone un barco en el espacio del tablero
                                {
                                    tablero[i, col] = "barco";
                                    espaciosop++;
                                }
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Error en el archivo (formato de barcos)");
                    Console.ReadKey();
                }
            }
            for (int i = 1; i < filas + 1; i++) //se llena el resto con "-"
            {
                for (int j = 1; j < columnas + 1; j++)
                {
                    if (oculto[i,j]==null)
                    {
                        oculto[i, j] = "  -  ";
                    }
                }
            }

        }
        public  void imprimirTablero(string [,] arr) //para imprimir tablero en consola
        {
            int rowLength = arr.GetLength(0);
            int colLength = arr.GetLength(1);
            for (int i = 0; i < rowLength; i++)
            {
                for (int j = 0; j < colLength; j++)
                {
                    Console.Write(string.Format("{0} ", arr[i, j]));
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
        }
        public string [] subArreglo (string [] arr, int index, int length) //para hacer el subarreglo de barcos
        {
            string[] result = new string[length];
            Array.Copy(arr, index, result, 0, length);
            return result;
        }
        public void atacar(string str) //se recibe el string de la posición para atacar
        {
            try
            {

                string[] array;
                array = str.Split(',');
                int fila = Convert.ToInt16(array[0]);
                int col = Convert.ToInt16(array[1]);
                if (fila<=filas&&col<=columnas) //se valida que sea un valor en las dimensiones establecidas
                {
                    if (oculto[fila,col].Equals("barco")) //si el tablero oculto es un barco se marca con x
                    {
                        oculto[fila, col] = "  x  ";
                        visible[fila, col] = "  x  ";
                        correcto++;
                    }
                    else //si no se marca con 0
                    {
                        oculto[fila, col] = "  0  ";
                        visible[fila, col] = "  0  ";
                        fallido++;
                    }
                }
                else
                {
                    Console.WriteLine("Número no válido");
                    Console.ReadKey();
                }
                Console.Clear();
                Console.WriteLine("TU TABLERO \n");
                imprimirTablero(tablero);
                Console.WriteLine("TABLERO DEL OPONENTE\n");
                imprimirTablero(visible);
            }
            catch
            {
                Console.WriteLine("Entrada no válido");
                Console.ReadKey();
            }
        }
        public void aleatorio(Random rnd) //se ataca aleatoriamente al jugador
        {
            int fila = 0;
            int col = 0;
            do
            {
                fila = rnd.Next(1, filas);
                col = rnd.Next(1, columnas);
            } while ((tablero[fila, col] == "  x  ") || (tablero[fila, col] == "  0  ")); //hasta que no se repita
            if (tablero[fila, col] == "barco")
            {
                correctoop++;
                tablero[fila, col] = "  x  ";
            }
            else if (tablero[fila, col] == "  -  ")
            {
                tablero[fila, col] = "  0  ";
            }
            Console.WriteLine("Turno del oponente, coordenadas: (" + Convert.ToString(fila) + "," + Convert.ToString(col) + ")\n PRESIONE ENTER PARA CONTINUAR");
            Console.ReadKey();
            
        }
        public bool verSiSigue()
        {
            
            if (fallido==3) //se valida si ya falló 3 veces
            {
                return false;
            }
            else if (correcto<espaciosop) //se valida si ya se completaron todos los barcos
            {
                if (correctoop <espacios)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }


    }
}
