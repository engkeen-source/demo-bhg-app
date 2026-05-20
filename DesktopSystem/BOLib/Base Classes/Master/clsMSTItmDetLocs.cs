

using System;
using System.Data;
using System.Data.SqlClient;
using Csla;
using Csla.Data;
using System.Transactions;
using BOLib;

[Serializable()]
public class MSTItmDetLocs : DataTable
{

    #region Factory Methods


    public MSTItmDetLocs()
    {
        this.Fetch(new Criteria(0, 1));
    }

    public MSTItmDetLocs(SqlConnection cn)
    {
        this.Fetch(cn, new Criteria(0, 1));
    }

    public static MSTItmDetLocs Get(int? headerKey)
    {
        MSTItmDetLocs obj = new MSTItmDetLocs();
        obj.Fetch(new Criteria(headerKey, 1));
        return obj;
    }

    public static MSTItmDetLocs New()
    {
        MSTItmDetLocs obj = new MSTItmDetLocs();
        return obj;
    }
    public static MSTItmDetLocs New(SqlConnection cn)
    {
        MSTItmDetLocs obj = new MSTItmDetLocs();
        obj.Fetch(cn, new Criteria(0, 1));
        return obj;
    }

    #endregion //Factory Methods

    #region Criteria

    [Serializable()]
    internal class Criteria
    {
        public int? _headerKey = null;
        public int? _option = null;

        internal Criteria()
        {
        }

        internal Criteria(int? HeaderKey, int? Option)
        {
            _headerKey = HeaderKey;
            _option = Option;
        }
    }

    #endregion //Criteria

    #region Data Access - Fetch

    internal bool Fetch(Criteria criteria)
    {
        bool retValue = false;

        // Create new sql connection for this method. 
        using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
        {
            // Open sql connection. 
            cn.Open();

            retValue = this.Fetch(cn, criteria);
        }// End of SqlConnection.



        return retValue;
    }

    internal bool Fetch(SqlConnection cn, Criteria criteria)
    {
        using (SqlCommand cm = cn.CreateCommand())
        {
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandText = "MSTItmDetLoc_Get";

            cm.Parameters.AddWithValue("@Option", criteria._option);
            cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey);
            // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
            cm.Parameters.AddWithValue("@RetValue", 0);
            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;

            System.Data.SqlClient.SqlDataAdapter sqlAdp = new System.Data.SqlClient.SqlDataAdapter(cm);
            sqlAdp.Fill(this);
            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                return true;
            else
                return false;

        }//using

    }

    #endregion //Data Access - Fetch

    #region Data Access - Insert

    internal bool Insert(int? headerKey)
    {
        bool retValue = false;

        using (TransactionScope scope = new TransactionScope())
        {
            // Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Insert(cn, headerKey);
            }
            // No errors - commit transaction
              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        }// Already close and dispose sql connection.

        return retValue;
    }

    internal bool Insert(SqlConnection cn, int? headerKey)
    {
        bool retValue = false;
        if (this.Rows.Count == 0)
        {
            return true;
        }
        foreach (DataRow dr in this.Rows)
        {
            if (dr.RowState == DataRowState.Deleted)
            {
                retValue = true;
                continue;
            }
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetLoc_AddUpdate";
                cm.Parameters.AddWithValue("@Option", 0);
                cm.Parameters.AddWithValue("@ItmKey", headerKey);
                cm.Parameters.AddWithValue("@LocKey", dr["LocKey"]);
                cm.Parameters.AddWithValue("@LocQty", dr["LocQty"].ToString() == "" ? 0 : dr["LocQty"]);
                cm.Parameters.AddWithValue("@LocQtyMin", dr["LocQtyMin"].ToString() == "" ? 0 : dr["LocQtyMin"]);
                cm.Parameters.AddWithValue("@LocQtyMax", dr["LocQtyMax"].ToString() == "" ? 0 : dr["LocQtyMax"]);
                cm.Parameters.AddWithValue("@LocQtyOpenBal", dr["LocQtyOpenBal"].ToString() == "" ? 0 : dr["LocQtyOpenBal"]);
                cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();
                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                {
                    retValue = true;
                }
            }// Already close and dispose sql command.
        }

        return retValue;
    }

    #endregion Insert

    #region Data Access - Update

    internal bool Update()
    {
        bool retValue = false;

        using (TransactionScope scope = new TransactionScope())
        {
            //Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Update(cn);
            }
            // No errors - commit transaction
              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        }// Already close and dispose sql connection.

        return retValue;
    }

    internal bool Update(SqlConnection cn)
    {
        bool retValue = false;
        if (this.Rows.Count == 0)
        {
            return true;
        }
        foreach (DataRow dr in this.Rows)
        {
            using (SqlCommand cm = cn.CreateCommand())
            {
                cm.CommandType = CommandType.StoredProcedure;
                cm.CommandText = "MSTItmDetLoc_AddUpdate";

                cm.Parameters.AddWithValue("@Option", 1);

                cm.Parameters.AddWithValue("@ItmKey", dr["ItmKey"]);
                cm.Parameters.AddWithValue("@LocKey", dr["LocKey"]);
                cm.Parameters.AddWithValue("@LocQty", dr["LocQty"].ToString() == "" ? 0 : dr["LocQty"]);
                cm.Parameters.AddWithValue("@LocQtyMin", dr["LocQtyMin"].ToString() == "" ? 0 : dr["LocQtyMin"]);
                cm.Parameters.AddWithValue("@LocQtyMax", dr["LocQtyMax"].ToString() == "" ? 0 : dr["LocQtyMax"]);
                cm.Parameters.AddWithValue("@LocQtyOpenBal", dr["LocQtyOpenBal"].ToString() == "" ? 0 : dr["LocQtyOpenBal"]);
                cm.Parameters.AddWithValue("@Custom1", dr["Custom1"]);
                cm.Parameters.AddWithValue("@Custom2", dr["Custom2"]);
                cm.Parameters.AddWithValue("@Custom3", dr["Custom3"]);
                cm.Parameters.AddWithValue("@CreateDate", DBNull.Value);
                cm.Parameters.AddWithValue("@CreateUserKey", AppInfor.currentUserKey);
                cm.Parameters.AddWithValue("@LastModifiedDate", DBNull.Value);
                cm.Parameters.AddWithValue("@LastModifiedUserKey", AppInfor.currentUserKey);

                cm.Parameters.AddWithValue("@RetValue", 0);
                cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
                // cm.Parameters["@NewDocKey"].Direction = ParameterDirection.Output;
                // Execute command.
                cm.ExecuteNonQuery();


                if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                    retValue = true;
                else
                    retValue = false;
            }
        }// Already close and dispose sql command.

        return retValue;
    }

    #endregion Update

    #region Data Access - Delete

    internal bool Delete(Criteria criteria)
    {
        bool retValue = false;

        using (TransactionScope scope = new TransactionScope())
        {
            //Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = this.Delete(cn, criteria);
            }
            // No errors - commit transaction
              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        }// Already close and dispose sql connection.

        return retValue;
    }

    internal bool Delete(SqlConnection cn, Criteria criteria)
    {
        using (SqlCommand cm = cn.CreateCommand())
        {
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandText = "MSTItmDetLoc_Delete";
            cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey);
            // Additional Parameter for Return Value From StoredProcedure -- Changed By Richard
            cm.Parameters.AddWithValue("@RetValue", 0);
            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
            // Execute command.
            cm.ExecuteNonQuery();

            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                return true;
            else
                return false;
        }// Already close and dispose sql command.            
    }

    #endregion Delete

    #region Data Access - Validation

    internal bool Validation(Criteria criteria, bool isNew)
    {
        bool retValue = false;
        using (TransactionScope scope = new TransactionScope())
        {
            //Create new sql connection for this method. 
            using (SqlConnection cn = new SqlConnection(Database.BossDemoConnection))
            {
                // Open sql connection. 
                cn.Open();
                retValue = Validation(cn, criteria, isNew);
            }
            // No errors - commit transaction
              if (Transaction.Current.TransactionInformation.Status != TransactionStatus.Active)  throw new Exception("Transaction has aborted."); scope.Complete();
        }// Already close and dispose sql connection.

        return retValue;
    }

    internal bool Validation(SqlConnection cn, Criteria criteria, bool isNew)
    {
        using (SqlCommand cm = cn.CreateCommand())
        {
            cm.CommandType = CommandType.StoredProcedure;
            cm.CommandText = "MSTItmDetLoc_Validation";

            cm.Parameters.AddWithValue("@isNew", isNew);

            cm.Parameters.AddWithValue("@ItmKey", criteria._headerKey);

            cm.Parameters.AddWithValue("@RetValue", 0);
            cm.Parameters["@RetValue"].Direction = ParameterDirection.Output;
            // Execute command.
            cm.ExecuteNonQuery();

            if ((int)cm.Parameters["@RetValue"].Value == (int)GEnum.SpState.Pass)
                return true;
            else
                return false;
        }// Already close and dispose sql command.

    }

    #endregion Validation
}


