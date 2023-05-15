using System;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        Console.Write("Enter the number of cities: ");
        int numCities = int.Parse(Console.ReadLine());

        Dictionary<int, City> cities = new Dictionary<int, City>();

        for (int i = 0; i < numCities; i++)
        {
            Console.WriteLine("City {i}:");
            Console.Write("Enter city name: ");
            string cityName = Console.ReadLine();
            Console.Write("Enter the number of cities connected to this city: ");
            int numConnections = int.Parse(Console.ReadLine());

            List<int> connections = new List<int>();

            for (int j = 0; j < numConnections; j++)
            {
                Console.Write("Enter the ID of the connected city {j + 1}: ");
                int cityId = int.Parse(Console.ReadLine());

                if (cityId == i || cityId >= numCities || connections.Contains(cityId))
                {
                    Console.WriteLine("Invalid ID");
                    j--;
                    continue;
                }

                connections.Add(cityId);
            }

            cities.Add(i, new City(cityName, connections));
        }

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("=== Current Status ===");

            foreach (var city in cities)
            {
                Console.WriteLine("City ID: {city.Key} | City Name: {city.Value.Name} | Outbreak Level: {city.Value.OutbreakLevel}");
            }

            Console.WriteLine();
            Console.WriteLine("Enter an event (Outbreak, Vaccinate, Lockdown, Spread, Exit): ");
            string inputEvent = Console.ReadLine();

            if (inputEvent == "Exit")
            {
                break;
            }
            else if (inputEvent == "Outbreak" || inputEvent == "Vaccinate" || inputEvent == "Lockdown")
            {
                Console.Write("Enter the ID of the city: ");
                int cityId = int.Parse(Console.ReadLine());

                if (!cities.ContainsKey(cityId))
                {
                    Console.WriteLine("Invalid ID");
                    continue;
                }

                if (inputEvent == "Outbreak")
                {
                    cities[cityId].IncreaseOutbreakLevel(2);
                    foreach (int connectedCityId in cities[cityId].Connections)
                    {
                        cities[connectedCityId].IncreaseOutbreakLevel(1);
                    }
                }
                else if (inputEvent == "Vaccinate")
                {
                    cities[cityId].ResetOutbreakLevel();
                }
                else if (inputEvent == "Lockdown")
                {
                    cities[cityId].DecreaseOutbreakLevel(1);
                    foreach (int connectedCityId in cities[cityId].Connections)
                    {
                        cities[connectedCityId].DecreaseOutbreakLevel(1);
                    }
                }
            }
            else if (inputEvent == "Spread")
            {
                foreach (var city in cities)
                {
                    bool increaseOutbreakLevel = false;
                    foreach (int connectedCityId in city.Value.Connections)
                    {
                        if (cities[connectedCityId].OutbreakLevel > city.Value.OutbreakLevel)
                        {
                            increaseOutbreakLevel = true;
                            break;
                        }
                    }

                    if (increaseOutbreakLevel)
                    {
                        city.Value.IncreaseOutbreakLevel(1);
                    }
                }
            }
            else
            {
                Console.WriteLine("Invalid");
                continue;
            }
        }
    }
}

class City
{
    public string Name { get; set; }
    public List<int> Connections { get; set; }
    public int OutbreakLevel { get; private set; }

    public City(string name, List<int> connections)
    {
        Name = name;
        Connections = connections;
        OutbreakLevel = 0;
    }

    public void IncreaseOutbreakLevel(int amount)
    {
        OutbreakLevel = Math.Min(OutbreakLevel + amount, 3);
    }

    public void DecreaseOutbreakLevel(int amount)
    {
        OutbreakLevel = Math.Max(OutbreakLevel - amount, 0);
    }

    public void ResetOutbreakLevel()
    {
        OutbreakLevel = 0;
    }
}