using EFCorePractice.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EFCorePractice
{
    class Program
    {
        static void Main(string[] args)
        {
            // select usrid, max(Name) Name,
            //(select iua.Address from UserAddress iua where iua.UserId = ot.usrId and iua.Type = 'HHHHHHHHHH') as HomeAddress,
            //(select iua.Address from UserAddress iua where iua.UserId = ot.usrId and iua.Type = 'OOOOOOOOOO') as OfficeAddress
            //from
            //(
            //select u.Id, u.Name, ua.Type, ua.Address, ua.UserId as usrId from
            //[User] AS u
            //left join UserAddress ua on u.Id = ua.UserId
            //) ot
            //group by ot.usrId

            EFPracticeContext dbContext = new EFPracticeContext();
            //var user = dbContext.User
            //    .Select(a => new 
            //    {
            //        id = a.Id,
            //        name = a.Name,
            //        usrAddress = a.UserAddress
            //        .AsQueryable()
            //        .Select(b => b.Address)
            //        .ToList()
            //    })
            //    .FirstOrDefault();

            //foreach (var addr in user.usrAddress)
            //    Console.WriteLine(addr);

            //var query = from usr in dbContext.User
            //            join addr in dbContext.UserAddress on usr.Id equals addr.UserId
            //            group usr by usr.Id into usrGroup
            //            select new
            //            {
            //                id = usrGroup.Key,
            //                Addresses =
            //                    usrGroup.Aggregate((a, b) =>
            //                        new { id = a.Id, Address = (a. + ", " + b.) }).ProdName
            //            };

            //ObjectSet<Contact> contacts = context.Contacts;
            //ObjectSet<SalesOrderHeader> orders = context.SalesOrderHeaders;
            //var query=dbContext.User.Join(UserAddress,u=>u.Id,)

            //var query = from usr in dbContext.User
            //            join addr in dbContext.UserAddress on usr.Id equals addr.UserId into grpAddress
            //            from ua in grpAddress.DefaultIfEmpty()
            //            group new
            //            {
            //                Id = usr.Id,
            //                Name = usr.Name,
            //                Type = ua.Type,
            //                Address = ua.Address
            //                //HomeAddress = ua.Type == "HHHHHHHHHH" ? ua.Address : string.Empty,
            //                //OfficeAddress = ua.Type == "OOOOOOOOOO" ? ua.Address : string.Empty
            //            } by usr.Id into grpUsr
            //            select grpUsr;

            //group usr by usr.Id into grpUsr
            //select
            //new 
            //{
            //    Id=usr.Id,
            //    Name=usr.Name,
            //    Type=ua.Type,
            //    Address=ua.Address
            //    //HomeAddress = ua.Type == "HHHHHHHHHH" ? ua.Address : string.Empty,
            //    //OfficeAddress = ua.Type == "OOOOOOOOOO" ? ua.Address : string.Empty
            //};


            var query = from usr in dbContext.User
                        join addr in dbContext.UserAddress on usr.Id equals addr.UserId into grpAddress
                        from ua in grpAddress.DefaultIfEmpty()
                        group usr by new { usr.Id,usr.Name }  into usrGroup
                        select new
                        {
                            Id = usrGroup.Key.Id,
                            Name = (from uGrp in usrGroup
                                    select uGrp.Name).Max()
                            //,Type =usrGroup.Key.Type
                            //Address = ua.Address
                            ,HomeAddress = (from iua in dbContext.UserAddress where iua.UserId== usrGroup.Key.Id && iua.Type== "HHHHHHHHHH" select iua.Address)
                            ,OfficeAddress = (from iua in dbContext.UserAddress where iua.UserId == usrGroup.Key.Id && iua.Type == "OOOOOOOOOO" select iua.Address)
                        };

            // var sqlQuery = QueryableHelper.ToSql(query);
            //List<UserInfo> usersInfo = new List<UserInfo>();
            //foreach (var item in query)
            //{
            //    usersInfo.Add(new UserInfo() { Id = item.Id, Name = item.Name, HomeAddress = item.Type == "HHHHHHHHHH" ? item.Address : string.Empty, OfficeAddress = item.Type== "OOOOOOOOOO" ? item.Address:string.Empty, Type = item.Type });
            //}

            IList<UserInfo> usersInfo = query.Select(x => new UserInfo() { Id = x.Id, Name = x.Name,HomeAddress=x.HomeAddress.Max(), OfficeAddress = x.OfficeAddress.Max() }).ToList();


            //IList<UserInfo> usersInfo = query.ToList();

            //usersInfo.ToList().ForEach(u =>
            //{
            //    if (u.Type == "HHHHHHHHHH")
            //        Console.WriteLine("Home Address:" + u.HomeAddress);
            //    else
            //        Console.WriteLine("Office Address:" + u.OfficeAddress);
            //}
            //);

            Console.ReadLine();
        }
    }
}
