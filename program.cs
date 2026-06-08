using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

namespace SistemaReservasHotel
{
    // =========================================================================
    // PUNTO DE ENTRADA: Clase Program (Interfaz de usuario por consola)
    // =========================================================================
    class Program
    {
        static void Main(string[] args)
        {
            Hotel miHotel = new Hotel();

            // Carga automática inicial obligatoria al arrancar la aplicación
            miHotel.CargarHabitacionesTXT();
            miHotel.CargarReservasTXT();

            bool salir = false;
            while (!salir) // Menú interactivo continuo
            {
                Console.Clear();
                Console.WriteLine("===== SISTEMA HOTEL =====");
                Console.WriteLine("1. Registrar habitación");
                Console.WriteLine("2. Mostrar habitaciones disponibles");
                Console.WriteLine("3. Crear reserva");
                Console.WriteLine("4. Cancelar reserva");
                Console.WriteLine("5. Guardar datos");
                Console.WriteLine("6. Cargar datos");
                Console.WriteLine("7. Mostrar reservas");
                Console.WriteLine("8. Salir");
                Console.Write("Seleccione una opción: ");

                try // Uso obligatorio de manejo de excepciones
                {
                    int opcion = int.Parse(Console.ReadLine()); // Lanza FormatException si se ingresan letras
                    
                    switch (opcion)
                    {
                        case 1:
                            RegistrarHabitacionUI(miHotel);
                            break;
                        case 2:
                            miHotel.MostrarHabitacionesDisponibles();
                            break;
                        case 3:
                            CrearReservaUI(miHotel);
                            break;
                        case 4:
                            CancelarReservaUI(miHotel);
                            break;
                        case 5:
                            miHotel.GuardarHabitacionesTXT();
                            miHotel.GuardarReservasTXT();
                            break;
                        case 6:
                            miHotel.CargarHabitacionesTXT();
                            miHotel.CargarReservasTXT();
                            break;
                        case 7:
                            miHotel.MostrarReservas();
                            break;
                        case 8:
                            salir = true;
                            Console.WriteLine("¡Gracias por utilizar el sistema!");
                            break;
                        default:
                            Console.WriteLine("Opción inválida. Intente de nuevo.");
                            break;
                    }
                }
                catch (FormatException) // Captura explícita de errores de conversión de datos
                {
                    Console.WriteLine("\n[Error]: Debe ingresar caracteres numéricos válidos en la opción del menú.");
                }
                catch (Exception ex) // Captura general de errores inesperados
                {
                    Console.WriteLine($"\n[Error Inesperado]: {ex.Message}");
                }
                finally // Bloque ejecutado siempre para pausar la consola antes de reiniciar el ciclo
                {
                    if (!salir)
                    {
                        Console.WriteLine("\nPresione cualquier tecla para continuar...");
                        Console.ReadKey();
                    }
                }
            }
        }

        // Métodos auxiliares de captura de datos por consola (UI)

        static void RegistrarHabitacionUI(Hotel hotel)
        {
            Console.WriteLine("\n--- Registrar Habitación ---");
            try
            {
                Console.Write("Ingrese número de habitación: ");
                int numero = int.Parse(Console.ReadLine());

                Console.Write("Ingrese tipo (Simple, Doble, Suite): ");
                string tipo = Console.ReadLine();

                Console.Write("Ingrese precio: ");
                decimal precio = decimal.Parse(Console.ReadLine());

                // Por defecto, una habitación nueva se registra disponible (true)
                hotel.AgregarHabitacion(new Habitacion(numero, tipo, precio, true));
            }
            catch (FormatException)
            {
                Console.WriteLine("[Error]: Formato de entrada incorrecto. El número y el precio deben ser numéricos.");
            }
        }

        static void CrearReservaUI(Hotel hotel)
        {
            Console.WriteLine("\n--- Crear Reserva ---");
            try
            {
                Console.Write("Ingrese nombre del cliente: ");
                string nombre = Console.ReadLine();
                Console.Write("Ingrese cédula: ");
                string cedula = Console.ReadLine();
                Console.Write("Ingrese teléfono: ");
                string telefono = Console.ReadLine();

                Console.Write("Ingrese habitación: ");
                int numHabitacion = int.Parse(Console.ReadLine());

                // Captura interactiva según el formato local del flujo esperado (dd/MM/yyyy)
                Console.Write("Ingrese fecha entrada (dd/MM/yyyy): ");
                DateTime entrada = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Console.Write("Ingrese fecha salida (dd/MM/yyyy): ");
                DateTime salida = DateTime.ParseExact(Console.ReadLine(), "dd/MM/yyyy", CultureInfo.InvariantCulture);

                Cliente cliente = new Cliente(nombre, cedula, telefono);
                hotel.CrearReserva(cliente, numHabitacion, entrada, salida);
            }
            catch (FormatException)
            {
                Console.WriteLine("[Error]: Error en la conversión de datos. Verifique que la habitación sea un número y las fechas cumplan el formato dd/MM/yyyy.");
            }
        }

        static void CancelarReservaUI(Hotel hotel)
        {
            Console.WriteLine("\n--- Cancelar Reserva ---");
            try
            {
                Console.Write("Ingrese el número de la habitación para cancelar su reserva: ");
                int numero = int.Parse(Console.ReadLine());
                hotel.CancelarReserva(numero);
            }
            catch (FormatException)
            {
                Console.WriteLine("[Error]: Debe ingresar un número de habitación entero y válido.");
            }
        }
    }

    // =========================================================================
    // REQUISITO 1: Clase Habitacion
    // =========================================================================
    public class Habitacion
    {
        public int Numero { get; set; }
        public string Tipo { get; set; }
        public decimal Precio { get; set; }
        public bool Disponible { get; set; }

        public Habitacion(int numero, string tipo, decimal precio, bool disponible)
        {
            Numero = numero;
            Tipo = tipo;
            Precio = precio;
            Disponible = disponible;
        }

        public void MostrarInformacion()
        {
            string estado = Disponible ? "Disponible" : "Ocupada";
            Console.WriteLine($"Habitación #{Numero} | Tipo: {Tipo} | Precio: ${Precio:N2} | Estado: {estado}");
        }
    }

    // =========================================================================
    // REQUISITO 2: Clase Cliente
    // =========================================================================
    public class Cliente
    {
        public string Nombre { get; set; }
        public string Cedula { get; set; }
        public string Telefono { get; set; }

        public Cliente(string nombre, string cedula, string telefono)
        {
            Nombre = nombre;
            Cedula = cedula;
            Telefono = telefono;
        }
    }

    // =========================================================================
    // REQUISITO 3: Clase Reserva
    // =========================================================================
    public class Reserva
    {
        public Cliente Cliente { get; set; }
        public Habitacion Habitacion { get; set; }
        public DateTime FechaEntrada { get; set; }
        public DateTime FechaSalida { get; set; }

        public Reserva(Cliente cliente, Habitacion habitacion, DateTime fechaEntrada, DateTime fechaSalida)
        {
            Cliente = cliente;
            Habitacion = habitacion;
            FechaEntrada = fechaEntrada;
            FechaSalida = fechaSalida;
        }

        public decimal CalcularTotal()
        {
            TimeSpan diferencia = FechaSalida - FechaEntrada;
            int dias = diferencia.Days;
            if (dias <= 0) dias = 1; // Control por si reservan en la misma fecha
            return dias * Habitacion.Precio;
        }

        public void MostrarReserva()
        {
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine($"Cliente: {Cliente.Nombre} | Cédula: {Cliente.Cedula} | Teléfono: {Cliente.Telefono}");
            Console.WriteLine($"Habitación: #{Habitacion.Numero} ({Habitacion.Tipo}) | Precio p/noche: ${Habitacion.Precio:N2}");
            Console.WriteLine($"Estadía: {FechaEntrada:dd/MM/yyyy} al {FechaSalida:dd/MM/yyyy} ({ (FechaSalida - FechaEntrada).Days } días)");
            Console.WriteLine($"Total Neto a Pagar: ${CalcularTotal():N2}");
            Console.WriteLine("------------------------------------------------------------------");
        }
    }

    // =========================================================================
    // REQUISITO 4: Clase Hotel (Lógica central del negocio y persistencia)
    // =========================================================================
    public class Hotel
    {
        public List<Habitacion> Habitaciones { get; set; }
        public List<Reserva> Reservas { get; set; }

        // Constantes con los nombres exactos exigidos en las pautas
        private readonly string archivoHabitaciones = "habitaciones.txt";
        private readonly string archivoReservas = "reservas.txt";

        public Hotel()
        {
            Habitaciones = new List<Habitacion>();
            Reservas = new List<Reserva>();
        }

        // MÉTODOS DE NEGOCIO

        public void AgregarHabitacion(Habitacion habitacion)
        {
            // Validación obligatoria: No permitir números repetidos
            if (Habitaciones.Exists(h => h.Numero == habitacion.Numero))
            {
                Console.WriteLine($"[Validación]: Error. Ya existe una habitación registrada con el número {habitacion.Numero}.");
                return;
            }
            Habitaciones.Add(habitacion);
            Console.WriteLine($"Habitación {habitacion.Numero} registrada con éxito.");
        }

        public void MostrarHabitacionesDisponibles()
        {
            Console.WriteLine("\n--- Habitaciones Disponibles ---");
            var disponibles = Habitaciones.FindAll(h => h.Disponible);

            if (disponibles.Count == 0)
            {
                Console.WriteLine("No se encontraron habitaciones disponibles en este momento.");
                return;
            }

            foreach (var hab in disponibles)
            {
                hab.MostrarInformacion();
            }
        }

        public void CrearReserva(Cliente cliente, int numeroHabitacion, DateTime entrada, DateTime salida)
        {
            // Validación obligatoria: No permitir fechas de salida menores que entrada
            if (entrada >= salida)
            {
                Console.WriteLine("[Validación]: La fecha de salida debe ser estrictamente posterior a la de entrada.");
                return;
            }

            Habitacion hab = Habitaciones.Find(h => h.Numero == numeroHabitacion);

            if (hab == null)
            {
                Console.WriteLine("[Validación]: La habitación seleccionada no existe en el catálogo.");
                return;
            }

            // Validación obligatoria: No permitir reservar habitaciones ocupadas
            if (!hab.Disponible)
            {
                Console.WriteLine("[Validación]: Operación cancelada. La habitación ya está ocupada.");
                return;
            }

            // Cambiar estado a no disponible y añadir la reserva a la colección en memoria
            hab.Disponible = false;
            Reserva nuevaReserva = new Reserva(cliente, hab, entrada, salida);
            Reservas.Add(nuevaReserva);

            Console.WriteLine("\nReserva creada exitosamente.");
            nuevaReserva.MostrarReserva();
        }

        public void CancelarReserva(int numeroHabitacion)
        {
            Reserva reserva = Reservas.Find(r => r.Habitacion.Numero == numeroHabitacion);

            if (reserva == null)
            {
                Console.WriteLine("[Validación]: No existe ninguna reserva activa registrada para la habitación dada.");
                return;
            }

            reserva.Habitacion.Disponible = true; // Liberar estado
            Reservas.Remove(reserva);
            Console.WriteLine($"La reserva de la habitación {numeroHabitacion} ha sido cancelada exitosamente.");
        }

        public void MostrarReservas()
        {
            Console.WriteLine("\n--- Listado Histórico de Reservas ---");
            if (Reservas.Count == 0)
            {
                Console.WriteLine("No existen registros de reservas activas en el sistema.");
                return;
            }
            foreach (var r in Reservas)
            {
                r.MostrarReserva();
            }
        }

        // MÉTODOS DE PERSISTENCIA (Manejo de archivos planos TXT)

        public void GuardarHabitacionesTXT()
        {
            try
            {
                // Uso de StreamWriter para sobrescribir el archivo de texto estructurado
                using (StreamWriter sw = new StreamWriter(archivoHabitaciones, false))
                {
                    foreach (var h in Habitaciones)
                    {
                        // Estructura limpia exacta: Numero, Tipo, Precio, Disponible
                        sw.WriteLine($"{h.Numero}, {h.Tipo}, {h.Precio.ToString(CultureInfo.InvariantCulture)}, {h.Disponible.ToString().ToLower()}");
                    }
                }
                Console.WriteLine("Datos de habitaciones guardados exitosamente en 'habitaciones.txt'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error al guardar habitaciones]: {ex.Message}");
            }
        }

        public void CargarHabitacionesTXT()
        {
            try
            {
                if (!File.Exists(archivoHabitaciones)) return;

                Habitaciones.Clear();
                // Uso de File.ReadAllLines para leer secuencialmente
                string[] lineas = File.ReadAllLines(archivoHabitaciones);

                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    string[] partes = linea.Split(',');
                    int numero = int.Parse(partes[0].Trim());
                    string tipo = partes[1].Trim();
                    decimal precio = decimal.Parse(partes[2].Trim(), CultureInfo.InvariantCulture);
                    bool disponible = bool.Parse(partes[3].Trim());

                    Habitaciones.Add(new Habitacion(numero, tipo, precio, disponible));
                }
                Console.WriteLine("Habitaciones cargadas correctamente desde el almacenamiento local.");
            }
            catch (FileNotFoundException) // Captura específica requerida
            {
                Console.WriteLine("[Aviso]: Archivo 'habitaciones.txt' no encontrado. Se creará uno nuevo al guardar.");
            }
            catch (FormatException) // Captura específica de datos corruptos
            {
                Console.WriteLine("[Error]: Formato de archivo corrupto o inválido al mapear 'habitaciones.txt'.");
            }
        }

        public void GuardarReservasTXT()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter(archivoReservas, false))
                {
                    foreach (var r in Reservas)
                    {
                        // Estructura exacta: Nombre, Cedula, Telefono, NumeroHabitacion, FechaEntrada(yyyy-MM-dd), FechaSalida(yyyy-MM-dd)
                        string fEntrada = r.FechaEntrada.ToString("yyyy-MM-dd");
                        string fSalida = r.FechaSalida.ToString("yyyy-MM-dd");
                        sw.WriteLine($"{r.Cliente.Nombre}, {r.Cliente.Cedula}, {r.Cliente.Telefono}, {r.Habitacion.Numero}, {fEntrada}, {fSalida}");
                    }
                }
                Console.WriteLine("Datos de reservas guardados exitosamente en 'reservas.txt'.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Error al guardar reservas]: {ex.Message}");
            }
        }

        public void CargarReservasTXT()
        {
            try
            {
                if (!File.Exists(archivoReservas)) return;

                Reservas.Clear();
                string[] lineas = File.ReadAllLines(archivoReservas);

                foreach (string linea in lineas)
                {
                    if (string.IsNullOrWhiteSpace(linea)) continue;

                    string[] partes = linea.Split(',');
                    string nombre = partes[0].Trim();
                    string cedula = partes[1].Trim();
                    string telefono = partes[2].Trim();
                    int numHabitacion = int.Parse(partes[3].Trim());
                    
                    // Conversión exacta en formato internacional año-mes-día usado en archivos planos
                    DateTime entrada = DateTime.ParseExact(partes[4].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);
                    DateTime salida = DateTime.ParseExact(partes[5].Trim(), "yyyy-MM-dd", CultureInfo.InvariantCulture);

                    // Re-vinculación asociativa con la habitación en memoria de forma relacional
                    Habitacion hab = Habitaciones.Find(h => h.Numero == numHabitacion);
                    if (hab != null)
                    {
                        Reservas.Add(new Reserva(new Cliente(nombre, cedula, telefono), hab, entrada, salida));
                    }
                }
                Console.WriteLine("Reservas cargadas correctamente desde el almacenamiento local.");
            }
            catch (FileNotFoundException)
            {
                Console.WriteLine("[Aviso]: Archivo 'reservas.txt' no encontrado. Se creará uno nuevo al guardar.");
            }
            catch (FormatException)
            {
                Console.WriteLine("[Error]: Formato de archivo corrupto o inválido al mapear 'reservas.txt'.");
            }
        }
    }
}
