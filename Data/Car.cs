
using System.ComponentModel.DataAnnotations;

namespace jwt_test.Models{
    public class Car{
        [Key]
        public int carId {get;set;}
        public string carName {get;set;}
        public int driverId {get;set;}
        public Driver driver {get;set;}
        
    }//end of car class
}//end of namespace