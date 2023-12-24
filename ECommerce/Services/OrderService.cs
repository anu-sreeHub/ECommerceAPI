using ECommerce.Interface;
using ECommerce.ViewModel;
using System.Data;
using System.Data.SqlClient;

namespace ECommerce.Services;

public class OrderService : IOrderService
{
    private readonly IDatabaseService _databaseService;

    public OrderService(IDatabaseService databaseService)
    {
        _databaseService = databaseService;  
    }

    public ReturnVM GetRecentCustomerOrder(string customerId, string email)
    {
        ReturnVM returnVM = new ReturnVM();
        try
        {
            using (SqlConnection connection = new SqlConnection(_databaseService.ConnectionString))
            {
                connection.Open();

                string query = $@";WITH RecentOrder AS (
                    SELECT TOP 1 O.ORDERID, O.ORDERDATE, C.ID AS CUSTOMERID
                    FROM CUSTOMERS AS C
		                LEFT OUTER JOIN ORDERS AS O ON C.ID = O.CUSTOMERID
                    WHERE C.CUSTOMERID = '{customerId}' AND C.EMAIL = '{email}'
                    ORDER BY O.ORDERDATE DESC
                )
                SELECT C.ID, C.FIRSTNAME, C.LASTNAME, O.ORDERID, O.ORDERDATE, C.HOUSENO + ' ' + C.STREET + ', ' + C.TOWN + ', ' + C.POSTCODE AS BILLINGADDRESS,
                O.DELIVERYEXPECTED, CASE O.CONTAINSGIFT WHEN 1 THEN 'Gift' ELSE P.PRODUCTNAME END PRODUCTNAME, OI.QUANTITY, OI.PRICE
                FROM CUSTOMERS AS C
                INNER JOIN RecentOrder AS RO ON C.ID = RO.CUSTOMERID
                LEFT OUTER JOIN ORDERS AS O ON RO.ORDERID = O.ORDERID
                LEFT OUTER JOIN ORDERITEMS AS OI ON O.ORDERID = OI.ORDERID
                LEFT OUTER JOIN PRODUCTS AS P ON OI.PRODUCTID = P.PRODUCTID
                ORDER BY O.ORDERDATE DESC;";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            DataTable dataTable = new DataTable();
                            // Load the SqlDataReader into the DataTable
                            dataTable.Load(reader);

                            // Perform group by on the "ID" column
                            var groupedData = from row in dataTable.AsEnumerable()
                                              group row by row.Field<int>("ID") into grp
                                              select new CustomerOrderVM
                                              {
                                                  Customer = new CustomerVM()
                                                  {
                                                      FirstName = grp.FirstOrDefault().Field<string>("FIRSTNAME"),
                                                      LastName = grp.FirstOrDefault().Field<string>("LASTNAME")
                                                  },
                                                  Order = grp.Any(item => item["ORDERID"] != DBNull.Value) ? new OrderVM()
                                                  {
                                                      OrderNo = grp.FirstOrDefault().Field<int>("ORDERID"),
                                                      OrderDate = grp.FirstOrDefault().Field<DateTime>("ORDERDATE"),
                                                      DeliveryAddress = grp.FirstOrDefault().Field<string>("BILLINGADDRESS"),
                                                      DeliveryExpected = grp.FirstOrDefault().Field<DateTime>("DELIVERYEXPECTED"),
                                                      OrderItems = grp.Select(item => new OrderItemsVM
                                                      {
                                                          Product = item.Field<string>("PRODUCTNAME"),
                                                          Quantity = item.Field<int>("QUANTITY"),
                                                          PriceEach = item.Field<decimal>("PRICE")
                                                      }).ToList()
                                                  } : null
                                              };

                            returnVM = new ReturnVM { IsSuccess = true, ReturnValue = groupedData, Message = "Success" };
                        }
                        else
                        {
                            returnVM = new ReturnVM { IsSuccess = false, Message = "Invalid Customer ID or Email ID!" };
                        }
                    }
                }
            }

        }
        catch (Exception ex)
        {
            // Handle exceptions
            Console.WriteLine($"Error: {ex.Message}");
        }
        return returnVM;

    }
}
