using MeccaNow.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MeccaNow.Helper
{
    public class DB
    {
        public void CreatePred(long Pred, string DisplayName)
        {
            try
            {
                using (SqlConnection SQLConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString))
                {
                    SQLConn.Open();
                    using (SqlCommand Comm = new SqlCommand("CreatePred", SQLConn))
                    {
                        Comm.CommandType = System.Data.CommandType.StoredProcedure;
                        Comm.Parameters.Add(new SqlParameter("@Pred", Pred));
                        Comm.Parameters.Add(new SqlParameter("@DisplayName", DisplayName));
                        

                        Comm.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
              
               
            }


        }

        public Response GetPred()
        {
            try
            {


                Response res = null;
                using (SqlConnection SQLConn = new SqlConnection(ConfigurationManager.ConnectionStrings["DBConnStr"].ConnectionString))
                {
                    SQLConn.Open();
                    using (SqlCommand Comm = new SqlCommand("GetPred", SQLConn))
                    {
                        Comm.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        SqlDataReader r = Comm.ExecuteReader();
                        if (r.HasRows)
                        {
                            res = new Response();
                            while (r.Read())
                            {
                                res.Pridection = Convert.ToInt32(r["Pridection"].ToString());
                                res.DisplayName = r["DisplayName"].ToString();
                               


                            }

                        }

                    }
                }
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


    }
}