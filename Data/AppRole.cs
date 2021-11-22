using Microsoft.AspNetCore.Identity;

namespace jwt_test.Data{
    public class AppRole : IdentityRole<int>{
        public AppRole(string Name): base(Name){}
    }//end of class
}//end of namespace