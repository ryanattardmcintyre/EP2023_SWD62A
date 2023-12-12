using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class Seat
    {
        public int Id { get; set; }

        public int Col { get; set; }
        public int Row { get; set; }

        public string RowCol { get {
                return Row + "," + Col;
            } }
    }


    public class SeatingPlanViewModel
    {
        public List<Seat> SeatingList { get; set; }
        public string SeatChosen { get; set; }



        public int MaxColumns { get; set; }
        public int MaxRows { get; set; }
    }

    public class UIGeneratedController : Controller
    {

        public IActionResult Create() {

            
             List<Seat> seatingList = new List<Seat>();

            int maxrows = 6;
            int maxcolumns = 10;


            for (int row = 0; row < 10; row++)
            {
                for (int col = 0; col < 10; col++)
                {
                    seatingList.Add(new Seat() { Row = row, Col=col });
                }

            }

            return View(new SeatingPlanViewModel() { SeatingList = seatingList, MaxColumns =maxcolumns, MaxRows=maxrows });
        
        }

        [HttpPost]
         public IActionResult Create(SeatingPlanViewModel model,  string selectedseat)
        {

            //process the details received
            return View();
        } 
    }
}
