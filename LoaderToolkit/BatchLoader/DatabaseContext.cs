using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace BatchLoader
{
    class DatabaseContext : IDisposable
    {
        private SqlConnection connection;
        private SqlTransaction transaction;

        public SqlConnection Connection
        {
            get { return connection; }
        }

        public SqlTransaction Transaction
        {
            get { return transaction; }
        }

        public DatabaseContext()
        {
            InitializeMembers();

            OpenConnection();
        }

        private void InitializeMembers()
        {
            this.connection = null;
            this.transaction = null;
        }

        public void Commit()
        {
            transaction.Commit();
            transaction.Dispose();
            transaction = null;
        }

        public void Dispose()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }

            if (connection != null)
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }
        }

        private void OpenConnection()
        {
            connection = new SqlConnection(AppSettings.AdminConnectionString);
            connection.Open();

            transaction = connection.BeginTransaction();
        }
    }
}
