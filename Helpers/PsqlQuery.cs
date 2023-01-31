using FinanceAppBackend.Models;
using FinanceAppBackend.Contexts;
using FinanceAppBackend.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using Npgsql;
using System.Collections.Generic;

namespace FinanceAppBackend.Contexts
{
    public class PsqlQuery
    {
        private readonly UserContext _context;
        
        public bool ConvertBool(string text)
        {
            if (text == "f")
            {
                return false;
            } else {
                return true;
            }
        }
        public List<User> GetUsers() {
            //  var cs = "Host=localhost;Username=bernardo;Password=12345678;Database=financeapp";
            var cs = "Host=ec2-54-164-40-66.compute-1.amazonaws.com;Database=db922knnim6pta;Username=yvjqlztnsclnhl;Password=ac44f6730e9a768054709163134f2f927ffae19c5f6ab17599aae9cbf5a3ab48;SSL Mode=Require;Trust Server Certificate=true";
            List<User> userList = new List<User>();

            using var con = new NpgsqlConnection(cs);
            con.Open();

            var sql = "SELECT * FROM users";

            using var cmd = new NpgsqlCommand(sql, con);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                User user = new User();
                user.Id = Int32.Parse(reader["Id"].ToString());
                user.Name = reader["name"].ToString();
                user.Email = reader["email"].ToString();
                user.Password = reader["password"].ToString();
                user.ProfileImage = reader["profileImage"].ToString();
                user.DarkMode = ConvertBool(reader["darkMode"].ToString());
                user.Notifications = ConvertBool(reader["notifications"].ToString());
                user.Balance = Double.Parse(reader["balance"].ToString());
                // and so on for each column
                userList.Add(user);
            }

            return userList;
        }
        public Stock Insert(Stock stock) {
            //  var cs = "Host=localhost;Username=bernardo;Password=12345678;Database=financeapp";
            var cs = "Host=ec2-54-164-40-66.compute-1.amazonaws.com;Database=db922knnim6pta;Username=yvjqlztnsclnhl;Password=ac44f6730e9a768054709163134f2f927ffae19c5f6ab17599aae9cbf5a3ab48;SSL Mode=Require;Trust Server Certificate=true";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO stocks (symbol, name, lastsale, netchange, pctchange, marketcap)  VALUES (@value1, @value2, @value3, @value4, @value5, @value6)";
            cmd.Parameters.Add(new NpgsqlParameter("@value1", stock.Symbol));
            cmd.Parameters.Add(new NpgsqlParameter("@value2", stock.Name));
            cmd.Parameters.Add(new NpgsqlParameter("@value3", stock.Lastsale));
            cmd.Parameters.Add(new NpgsqlParameter("@value4", stock.Netchange));
            cmd.Parameters.Add(new NpgsqlParameter("@value5", stock.Pctchange));
            cmd.Parameters.Add(new NpgsqlParameter("@value6", stock.Marketcap));
            cmd.ExecuteNonQuery();

            return GetStock(stock.Symbol);
        }
        public Stock Refresh(Stock stock) {
            //  var cs = "Host=localhost;Username=bernardo;Password=12345678;Database=financeapp";
            var cs = "Host=ec2-54-164-40-66.compute-1.amazonaws.com;Database=db922knnim6pta;Username=yvjqlztnsclnhl;Password=ac44f6730e9a768054709163134f2f927ffae19c5f6ab17599aae9cbf5a3ab48;SSL Mode=Require;Trust Server Certificate=true";

            List<User> userList = new List<User>();

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "UPDATE Stocks SET symbol=@symbol, name=@name, lastsale=@lastsale, netchange=@netchange, pctchange=@pctchange, marketcap=@marketcap WHERE symbol=@symbol";
            cmd.Parameters.Add(new NpgsqlParameter("@symbol", stock.Symbol));
            cmd.Parameters.Add(new NpgsqlParameter("@name", stock.Name));
            cmd.Parameters.Add(new NpgsqlParameter("@lastsale", stock.Lastsale));
            cmd.Parameters.Add(new NpgsqlParameter("@netchange", stock.Netchange));
            cmd.Parameters.Add(new NpgsqlParameter("@pctchange", stock.Pctchange));
            cmd.Parameters.Add(new NpgsqlParameter("@marketcap", stock.Marketcap));
            cmd.ExecuteNonQuery();

            return GetStock(stock.Symbol);
        }
        public Stock GetStock(string symbol) {
            //  var cs = "Host=localhost;Username=bernardo;Password=12345678;Database=financeapp";
            var cs = "Host=ec2-54-164-40-66.compute-1.amazonaws.com;Database=db922knnim6pta;Username=yvjqlztnsclnhl;Password=ac44f6730e9a768054709163134f2f927ffae19c5f6ab17599aae9cbf5a3ab48;SSL Mode=Require;Trust Server Certificate=true";

            List<Stock> stockList = new List<Stock>();

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Stocks WHERE symbol=@symbol";
            cmd.Parameters.Add(new NpgsqlParameter("@symbol", symbol));
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Stock stock = new Stock();
                stock.Id = Int32.Parse(reader["id"].ToString());
                stock.Symbol = reader["symbol"].ToString();
                stock.Name = reader["name"].ToString();
                stock.Lastsale = Double.Parse(reader["lastsale"].ToString().Replace("$",""));
                stock.Pctchange = Double.Parse(reader["pctchange"].ToString().Replace("%",""))/100;
                stock.Netchange = Double.Parse(reader["netchange"].ToString());
                stock.Marketcap = Double.Parse(reader["marketcap"].ToString().Replace(",",""));
                // and so on for each column
                stockList.Add(stock);
            }
            if (stockList.Count()>0) {
                return stockList[0];
            } else {
                return new Stock {};
            }
        }
        public Order Create(OrderDto order) {
            //  var cs = "Host=localhost;Username=bernardo;Password=12345678;Database=financeapp";
            var cs = "Host=ec2-54-164-40-66.compute-1.amazonaws.com;Database=db922knnim6pta;Username=yvjqlztnsclnhl;Password=ac44f6730e9a768054709163134f2f927ffae19c5f6ab17599aae9cbf5a3ab48;SSL Mode=Require;Trust Server Certificate=true";

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "INSERT INTO orders (type, quantity, price, user, asset)  VALUES (@type, @quantity, @price, @user, @asset)";
            cmd.Parameters.Add(new NpgsqlParameter("@type", order.Type));
            cmd.Parameters.Add(new NpgsqlParameter("@quantity", order.Quantity));
            cmd.Parameters.Add(new NpgsqlParameter("@price", order.Price));
            cmd.Parameters.Add(new NpgsqlParameter("@user", order.User));
            cmd.Parameters.Add(new NpgsqlParameter("@asset", order.Asset));
            cmd.ExecuteNonQuery();

            return new Order{};
        }

        public List<Stock> Update(List<StockDto> list) {
            List<Stock> stocklist = new List<Stock>();

            for (int i=0; i<list.Count();i++)
            {   
                var stock = new Stock {
                    Symbol = list[i].Symbol,
                    Name = list[i].Name,
                    Lastsale = Double.Parse(list[i].Lastsale.Replace("$","")),
                    Pctchange = Double.Parse(list[i].Pctchange.Replace("%",""))/100,
                    Netchange = Double.Parse(list[i].Netchange.Replace(",","")),
                    Marketcap = Double.Parse(list[i].Marketcap.Replace(",","")),
                };
                var test = GetStock(stock.Symbol);
                if (test.Id == 0) {
                    stocklist.Add(Insert(stock));
                } else {
                    stocklist.Add(Refresh(stock));                   
                }
            }

            return stocklist;
        }
        public List<Stock> Search(string text) {
            //  var cs = "Host=localhost;Username=bernardo;Password=12345678;Database=financeapp";
            var cs = "Host=ec2-54-164-40-66.compute-1.amazonaws.com;Database=db922knnim6pta;Username=yvjqlztnsclnhl;Password=ac44f6730e9a768054709163134f2f927ffae19c5f6ab17599aae9cbf5a3ab48;SSL Mode=Require;Trust Server Certificate=true";

            List<Stock> stockList = new List<Stock>();

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM Stocks WHERE symbol ~* @search OR name ~* @search";
            cmd.Parameters.Add(new NpgsqlParameter("@search", (text)));
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                Stock stock = new Stock();
                stock.Id = Int32.Parse(reader["id"].ToString());
                stock.Symbol = reader["symbol"].ToString();
                stock.Name = reader["name"].ToString();
                stock.Lastsale = Double.Parse(reader["lastsale"].ToString().Replace("$",""));
                stock.Pctchange = Double.Parse(reader["pctchange"].ToString().Replace("%",""))/100;
                stock.Netchange = Double.Parse(reader["netchange"].ToString());
                stock.Marketcap = Double.Parse(reader["marketcap"].ToString().Replace(",",""));
                // and so on for each column
                stockList.Add(stock);
            }
            if (stockList.Count()>0) {
                return stockList;
            } else {
                return new List<Stock>();
            }
        }
        public List<FetchOrderDto> Fetch(int userId) {
            //  var cs = "Host=localhost;Username=bernardo;Password=12345678;Database=financeapp";
            var cs = "Host=ec2-54-164-40-66.compute-1.amazonaws.com;Database=db922knnim6pta;Username=yvjqlztnsclnhl;Password=ac44f6730e9a768054709163134f2f927ffae19c5f6ab17599aae9cbf5a3ab48;SSL Mode=Require;Trust Server Certificate=true";
            
            List<FetchOrderDto> orderList = new List<FetchOrderDto>();

            using var con = new NpgsqlConnection(cs);
            con.Open();

            using var cmd = new NpgsqlCommand();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM orders JOIN stocks ON orders.asset = stocks.id WHERE orders.user = @userId";
            cmd.Parameters.Add(new NpgsqlParameter("@userId", (userId)));
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                FetchOrderDto fullorder = new FetchOrderDto();
                fullorder.Id = Int32.Parse(reader["id"].ToString());
                fullorder.Type = reader["type"].ToString();
                fullorder.Date = reader["date"].ToString();
                fullorder.Quantity = Int32.Parse(reader["quantity"].ToString());
                fullorder.Price = Double.Parse(reader["price"].ToString());
                fullorder.User = Int32.Parse(reader["user"].ToString());
                fullorder.Asset = Int32.Parse(reader["asset"].ToString());
                fullorder.Symbol = reader["symbol"].ToString();
                fullorder.Name = reader["name"].ToString();
                fullorder.Lastsale = Double.Parse(reader["lastsale"].ToString().Replace("$",""));
                fullorder.Pctchange = Double.Parse(reader["pctchange"].ToString().Replace("%",""))/100;
                fullorder.Netchange = Double.Parse(reader["netchange"].ToString());
                fullorder.Marketcap = Double.Parse(reader["marketcap"].ToString().Replace(",",""));
                // and so on for each column
                orderList.Add(fullorder);
            }
            if (orderList.Count()>0) {
                return orderList;
            } else {
                return new List<FetchOrderDto>();
            }
        }
    }
}