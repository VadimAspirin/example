using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Program
{
    class Storekeeper : User
    {
        public Storekeeper(string name = "Storekeeper")
        {
            tables = new DataSet(name);
            UpdateTables();
        }

        public override void UpdateTables()
        {
            flagConnection = true;

            var product = AsyncClient.SendMessage("select * from s_product");
            var typeProduct = AsyncClient.SendMessage("select * from s_type_product");
            if (product.TableName == "<ERROR>" || typeProduct.TableName == "<ERROR>")
            {
                flagConnection = false;
                return;
            }
            product.TableName = "s_product";
            product = newColumn(product, typeProduct, "n_id_type_product", "Тип", "c_name_type_product");
            product.Columns["n_id_product"].ColumnName = "Идентификатор";
            product.Columns["c_name_product"].ColumnName = "Название";
            product.Columns["n_product_count"].ColumnName = "Количество";
            product.Columns["n_product_price"].ColumnName = "Цена";
            product.Columns.Remove("n_id_type_product");

            typeProduct.TableName = "s_type_product";
            typeProduct.Columns["n_id_type_product"].ColumnName = "Идентификатор";
            typeProduct.Columns["c_name_type_product"].ColumnName = "Название";

            var bufDataSet = new DataSet();
            bufDataSet.Tables.Add(product);
            bufDataSet.Tables.Add(typeProduct);
            tables = bufDataSet;
        }

        //**************************************************************

        private bool checkPriceData(string price)
        {
            try
            {
                string[] bufString = price.Split(new Char[] { '.' });
                int bufInt = Convert.ToInt32(bufString[0]);
                if (bufInt <= 0)
                    return false;
                if (bufString.Length > 2)
                    return false;
                if (bufString.Length == 2)
                    bufInt = Convert.ToInt32(bufString[1]);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public bool AddProduct(string name, string price, string idTypeProduct)
        {
            if (!checkPriceData(price))
                return false;

            var buf = AsyncClient.SendMessage("insert into s_product (c_name_product, n_product_count, n_product_price, n_id_type_product) values ('" + name + "', '0', '" + price + "', '" + idTypeProduct + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool ChangeProduct(string id, string name, string price, string idTypeProduct)
        {
            if (!checkPriceData(price))
                return false;

            var buf = AsyncClient.SendMessage("update s_product set c_name_product='" + name + "', n_product_price='" + price + "', n_id_type_product='" + idTypeProduct + "' where n_id_product='" + id + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool DelProduct(string id)
        {
            var buf = AsyncClient.SendMessage("delete from s_product where n_id_product='" + id + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        //**************************************************************

        public bool AddTypeProduct(string name)
        {
            var buf = AsyncClient.SendMessage("insert into s_type_product (c_name_type_product) values ('" + name + "')");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

        public bool DelTypeProduct(string id)
        {
            var buf = AsyncClient.SendMessage("delete from s_type_product where n_id_type_product='" + id + "'");
            if (buf.TableName == "<ERROR>")
                return false;
            return true;
        }

    }
}
