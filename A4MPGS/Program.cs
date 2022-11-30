/*
 * Program - Assignment 4 Car Inventory
 * 
 * Purpose - to add, edit, delete and display the inventory of cars
 * 
 * Revision History - 
 *  Created by Miles Purvis and Gabe Siewert
 *  November 23, 2022
 *  
 */
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace A4MPGS
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Declare variables
            string[] brandPurvisSiewert = new string[3];
            string[] carInventory = new string[20];
            string input;
            bool exitLoop = true;

            Console.WriteLine("[Enter 3 Automotive Brands]\n");
            //For loop to prompt user for name of 3 brands
            for (int i = 0; i < brandPurvisSiewert.Length; i++)
            {
                Console.Write("Car Brand {0}: ", i + 1);
                brandPurvisSiewert[i] = Console.ReadLine();
            }

            Console.Clear();

            do
            {
                //Display Menu
                Console.WriteLine("[Menu]\n");
                Console.WriteLine(" A) Add new Car details");
                Console.WriteLine(" B) Edit existing Car details");
                Console.WriteLine(" C) Display all Cars in store");
                Console.WriteLine(" D) Delete Car Information");
                Console.WriteLine(" E) Exit the program");
                Console.Write(" \nSelect option (A, B, C, D, E) & Press Enter: ");
                input = Console.ReadLine();

                // ToUpper input for switch case
                input = input.ToUpper();

                Console.Clear();
                // Switch Case directs to each method
                switch (input)
                {
                    case "A":
                        NewCarDetails(brandPurvisSiewert, carInventory);
                        break;
                    case "B":
                        EditCarDetails(carInventory, brandPurvisSiewert);
                        break;
                    case "C":
                        DisplayCarDetails(carInventory);
                        break;
                    case "D":
                        DeleteCarDetails(carInventory);
                        break;
                    case "E":
                        exitLoop = false;
                        break;
                }

                //loop while you havent exited the loop
            } while (exitLoop == true);

        }

        //Method Adds car details.
        private static void NewCarDetails(string[] brandPurvisSiewert, string[] carInventory)
        {
            //Declare variables
            string brandName = "";
            string brandModel;
            bool brandExists = false;
            bool notAdded = false;
            int modelNum;
            int carNumber;
            int counter = 0;

            //carNumber = number of cars in the inventory != "NONE" || Emptystring/Null
            carNumber = CarCounter(carInventory);

            //Repeat option A until 20 records are entered or until DONE is entered in the brand field (WHILE LOOP)

            while (carNumber == 20 || counter != 20)
            {
                bool isValidCar = true;

                do
                {
                    //Brand Name and Model are taken using 2 strings
                    Console.WriteLine("Car Brand: {0}", brandPurvisSiewert[0]);
                    Console.WriteLine("Car Brand: {0}", brandPurvisSiewert[1]);
                    Console.WriteLine("Car Brand: {0}", brandPurvisSiewert[2]);
                    Console.Write("Enter one of the following Car brands or type \"DONE\" for the menu: ");
                    brandName = Console.ReadLine();

                    brandExists = CheckIfStringExists(brandPurvisSiewert, brandName);

                    //Brand Name is valid - must be one of the three brands in carBrands 
                    //if car brand doesnt exist and is ! "DONE"
                    if (brandExists == false && brandName != "DONE")
                    {
                        //if not valid ask to re-enter
                        Console.WriteLine("\n[Must re-enter valid car brand]");
                        isValidCar = false;

                    }
                    // exsits and 
                    else
                    {
                        isValidCar = true;
                    }

                    //Loop while car brand doesnt exist
                } while (isValidCar == false);


                //if brandName  == "DONE" return to main menu
                if (brandName == "DONE")
                {
                    Console.Clear();
                    return;
                }

                do
                {
                    modelNum = 0;
                    try
                    {
                        //models must be 1,2,3,4 
                        //then prompt user to enter model
                        Console.Write("\nEnter Brand model 1-4: ");
                        modelNum = int.Parse(Console.ReadLine());

                        if (modelNum < 1 || modelNum > 4)
                        {
                            //throw exception if model is <1 or >4
                            throw new FormatException("\nNumbers out of range must be 1-4");
                        }
                        bool exists = CheckIfStringExists(carInventory, brandName + "-" + modelNum);
                        if (exists == true)
                        {
                            //a boolean not added
                            notAdded = true;
                            throw new Exception("Model exists");

                        }
                        else
                        {
                            notAdded = false;
                        }
                    }
                    catch (FormatException fEx)
                    {
                        Console.WriteLine(fEx.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);

                        break;
                    }

                    //loop while num is <1 or >4 or if or if the car already exists
                } while ((modelNum < 1 || modelNum > 4) || CheckIfStringExists(carInventory, brandName + "-" + modelNum) == true);

                //Brand Name and Model are taken using 2 strings
                brandModel = modelNum.ToString();

                //for each car in the inventory if it is equal to "NONE" or is empty enter the car in this position
                for (int i = 0; i < carInventory.Length; i++)
                {
                    if ((carInventory[i] == "NONE" || string.IsNullOrEmpty(carInventory[i])) && notAdded == false)
                    {
                        //brand + model are concatenated correctly
                        carInventory[i] = brandName + "-" + brandModel;

                        //user notified that it is saved
                        Console.WriteLine("\nCar saved Add Another Car, Press Enter");
                        counter++;
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    }
                    else if (notAdded == true)
                    {
                        Console.WriteLine("\nCar Not added, Already Exists");
                        Console.ReadKey();
                        counter++;
                        Console.Clear();
                        break;
                    }
                    {
                    }
                }

            }
        }

        //Method allows you to edit car details.
        private static void EditCarDetails(string[] carInventory, string[] brandPurvisSiewert)
        {
            //Edit Brand
            string editBrandModel;

            //New Brand
            string newBrand = "";
            string newModel;

            // Index position of brand to edit
            int indexPosition;

            //Input for model
            int modelNum;

            //bool for validity
            bool brandExists;
            bool isValidCar;
            bool carExists;

            //Ask user to enter car
            Console.WriteLine("[Please Enter A Car to Edit (Brand-Model)]\n");

            //take input for what brand to edit
            Console.Write("Enter Brand Name and Model to edit (Brand-Model): ");
            editBrandModel = Console.ReadLine();

            Console.Clear();

            //check if those brands exsits, take the index position for that brand if it exsits
            carExists = CheckIfStringExists(carInventory, editBrandModel);
            indexPosition = FindCarIndex(carInventory, editBrandModel);

            if (carExists == true)
            {
                //if brand exsits 
                do
                {
                    //take brand from list of brands.
                    Console.WriteLine("[Changing: ({0}) Enter New Brand Name]\n", editBrandModel);
                    Console.WriteLine("Car Brand: {0}", brandPurvisSiewert[0]);
                    Console.WriteLine("Car Brand: {0}", brandPurvisSiewert[1]);
                    Console.WriteLine("Car Brand: {0}", brandPurvisSiewert[2]);
                    Console.Write("\nEnter one of the following Car Brands: ");
                    newBrand = Console.ReadLine();

                    //check if that brand is a valid brand
                    brandExists = CheckIfStringExists(brandPurvisSiewert, newBrand);

                    //if the brand doesnt exsits it is an invalid brand
                    if (brandExists == false)
                    {
                        //ask teh user to reenter valid car brand 1 or the 3
                        Console.WriteLine("\n[Must re-enter valid car brand]");
                        isValidCar = false;
                    }
                    //else this brand is one of the brands
                    else
                    {
                        isValidCar = true;
                    }

                    // exit loop if car is valid
                } while (isValidCar == false);

                do
                {
                    modelNum = 0;

                    try
                    {
                        //take input 1-4
                        Console.Write("\nEnter Brand model 1-4: ");
                        modelNum = int.Parse(Console.ReadLine());

                        // if model is <1 or >4 throw error that the car is not valid
                        if (modelNum < 1 || modelNum > 4)
                        {
                            throw new Exception("\nNumbers out of range must be 1-4");
                        }
                        // Check if the car brand and model exsits in carInventory
                        bool exists = CheckIfStringExists(carInventory, newBrand + "-" + modelNum);

                        //if the car exsits change that car to new car
                        if (exists == false)
                        {
                            newModel = modelNum.ToString();
                            carInventory[indexPosition] = newBrand + "-" + newModel;
                            Console.WriteLine("\nCar {0} saved, Press Enter to return to menu", newBrand + "-" + newModel);
                            Console.ReadKey();
                            Console.Clear();
                            break;
                        }
                    }
                    catch(FormatException fEx)
                    {
                        Console.WriteLine(fEx.Message);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                    }

                    //loop while the model number is valid or if the car doesnt exist
                } while ((modelNum < 1 || modelNum > 4) || CheckIfStringExists(carInventory, newBrand + "-" + modelNum) == false);
            }
            // else print brand record Not Found
            else if (carExists == false)
            {
                Console.WriteLine("Brand Record Not Found, Any Key to return to Menu");
                Console.ReadKey();
                Console.Clear();
            }

        }

        //Method Deletes car details
        private static void DeleteCarDetails(string[] carInventory)
        {
            //Declare Variables
            string delBrandModel;
            string delete;
            int indexPosition;
            bool exists = false;

            Console.WriteLine("[Enter car to delete]\n");

            //Take car details to delete (Brand-Model)
            Console.Write("Enter Car to Delete (Brand-Model): ");
            delBrandModel = Console.ReadLine();

            //Checks if string exsits and finds index of that car
            exists = CheckIfStringExists(carInventory, delBrandModel);
            indexPosition = FindCarIndex(carInventory, delBrandModel);

            //If it exists
            if (exists == true)
            {
                //Delete Y/N
                Console.WriteLine("Would you like to delete {0}? (y/n)", delBrandModel);
                delete = Console.ReadLine();

                delete = delete.ToUpper();

                switch (delete)
                {
                    //IndexPosition = NONE and displays car is deleted
                    case "Y":
                        carInventory[indexPosition] = "NONE";
                        Console.WriteLine("\nCar has been deleted! any key to return to menu");
                        Console.ReadKey();
                        Console.Clear();
                        break;
                    // Index Position not changed car not changed.
                    case "N":
                        Console.WriteLine("\nCar has not been removed, any key to return to menu");
                        Console.ReadKey();
                        Console.Clear();
                        return;

                    default:
                        Console.WriteLine("[\nInvalid Entry must be y/n]");
                        return;
                }

            }
            else if (exists == false)
            {
                //if no record exists display "Brand Entry Not Found" then return;
                Console.WriteLine("Brand Entry Not Found Press Enter to Return to Menu");
                Console.ReadKey();
                Console.Clear();
                return;
            }
        }

        //Method Displays all car details 
        private static void DisplayCarDetails(string[] carInventory)
        {
            Console.WriteLine("[Car Inventory]\n");
            //loop through car inventory and display the car inventory
            for (int i = 0; i < carInventory.Length; i++)
            {
                Console.WriteLine(carInventory[i]);
            }
            //enter to return to menu
            Console.WriteLine("Press Enter to Return to main menu");
            Console.ReadKey();
            Console.Clear();
            return;
        }

        //Method checks validity of string and array
        private static bool CheckIfStringExists(string[] stringArr, string str)
        {

            for (int i = 0; i < stringArr.Length; i++)
            {
                if (stringArr[i] == str)
                {
                    return true;
                }
            }
            return false;
        }

        //Method checks count of cars
        private static int CarCounter(string[] invArr)
        {
            int x = 0;
            for (int i = 0; i < invArr.Length; i++)
            {
                if (invArr[i] != "NONE" && string.IsNullOrEmpty(invArr[i]) == false)
                {
                    x = x + 1;
                }
            }
            return x;
        }

        //Method finds index position of car
        private static int FindCarIndex(string[] stringArr, string str)
        {
            for (int i = 0; i < stringArr.Length; i++)
            {
                if (stringArr[i] == str)
                {
                    return i;
                }
            }
            return -1;
        }

    }
}




