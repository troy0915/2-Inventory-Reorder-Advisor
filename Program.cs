using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Item
{
    public string Code { get; private set; }
    public string Name { get; private set; }
    public double UnitPrice { get; private set; }
    public int CurrentStock { get; private set; }
    public int MinStock { get; private set; }
    public int[] SalesLast7Days { get; private set; }
    public int LeadTime { get; private set; } // in days

    public Item(string code, string name, double unitPrice, int currentStock, int minStock, int[] salesLast7Days, int leadTime)
    {
        Code = code;
        Name = name;
        UnitPrice = unitPrice;
        CurrentStock = currentStock;
        MinStock = minStock;
        SalesLast7Days = salesLast7Days;
        LeadTime = leadTime;
    }
    public bool NeedsReorder()
    {
        double avgDailySales = ComputeMovingAverage();
        return (avgDailySales * LeadTime) > CurrentStock;
    }
    private double ComputeMovingAverage()
    {
        return SalesLast7Days.Average();
    }

    public double UrgencyScore()
    {
        double avgDailySales = ComputeMovingAverage();
        return (avgDailySales * LeadTime) - CurrentStock;
    }
}

namespace _2__Inventory_Reorder_Advisor
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Item> items = new List<Item>();
            Console.Write("Enter the number of items: ");
            int itemCount = int.Parse(Console.ReadLine());

            for (int i = 0; i < itemCount; i++)
            {
                Console.WriteLine($"\nEntering details for item {i + 1}:");
                string code = GetStringInput("Enter item code: ");
                string name = GetStringInput("Enter item name: ");
                double unitPrice = GetPositiveDoubleInput("Enter unit price: ");
                int currentStock = GetPositiveIntInput("Enter current stock: ");
                int minStock = GetPositiveIntInput("Enter minimum stock: ");
                int leadTime = GetPositiveIntInput("Enter lead time (in days): ");
                int[] salesLast7Days = GetSalesLast7Days();

                items.Add(new Item(code, name, unitPrice, currentStock, minStock, salesLast7Days, leadTime));
            }
            DisplayReorderList(items);
        }

        static string GetStringInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }

        static double GetPositiveDoubleInput(string prompt)
        {
            double value;
            while (true)
            {
                Console.Write(prompt);
                if (double.TryParse(Console.ReadLine(), out value) && value >= 0)
                {
                    return value;
                }
                Console.WriteLine("Invalid input. Please enter a positive number.");
            }
        }
        static int GetPositiveIntInput(string prompt)
        {
            int value;
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out value) && value >= 0)
                {
                    return value;
                }
                Console.WriteLine("Invalid input. Please enter a positive integer.");
            }
        }
        static int[] GetSalesLast7Days()
        {
            int[] sales = new int[7];
            for (int i = 0; i < 7; i++)
            {
                sales[i] = GetPositiveIntInput($"Enter sales for day {i + 1}: ");
            }
            return sales;
        }

        static void DisplayReorderList(List<Item> items)
        {
            var reorderList = items.Where(item => item.NeedsReorder())
                                    .OrderBy(item => item.UrgencyScore())
                                    .ToList();
            Console.WriteLine("\nReorder List:");
            foreach (var item in reorderList)
            {
                Console.WriteLine($"Code: {item.Code}, Name: {item.Name}, Urgency Score: {item.UrgencyScore():F2}");
            }
        }
    }
}





