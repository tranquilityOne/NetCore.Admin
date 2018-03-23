using e3net.Common.Entity;
using PetaPoco;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PetaPocoServer
{
    /// <summary>
    /// 数据访问基础接口
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseDAL<T>
    {

        #region 自定义

        /// <summary>
        /// 获取操作对像
        /// </summary>
        /// <returns></returns>
        Database GetDatabase();
        /// <summary>
        ///执行查询，并返回结果集的第一行的第一列
        /// </summary>
        /// <param name="sql">An SQL builder object representing the query and it's arguments</param>
        /// <returns>The scalar value cast to T</returns>
        object ZExecuteScalar(Sql sql);


        /// <summary>
        ///条件 获取数量
        /// </summary>
        /// <param name="whereStri">where 后面的条件</param>
        /// <returns></returns>
        long ZGetCount(string whereStri);

        /// <summary>
        /// 动态查询，返回dynamic类型的列表
        /// 请使用标准SQL语句进行查询(SELECT ... FROM ...)
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>

        /// <summary>
        /// 执行sql 返回dataTable
        /// </summary>
        /// <param name="Sql">完整的sql</param>
        /// <returns></returns>
        DataTable sqlToDataTable(Sql sql);


        /// <summary>
        /// 执行sql 返回翻页的  dataTable
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <param name="TotalPages">总页数</param>
        /// <param name="TotalItems">总数</param>
        /// <param name="Sql">sql语句</param>
        /// <returns></returns>
        DataTable sqlToDataTablePage(long page, long itemsPerPage, out long TotalPages, out long TotalItems, Sql sql);

        /// <summary>
        /// 执行sql 返回翻页的  dataTable
        /// </summary>
        /// <param name="pc">分页封装</param>
        /// <returns></returns>
        DataTable sqlToDataTablePage(PageClass pc);
        #endregion

        #region operation: Execute
        /// <summary>
        /// 执行sql 返回 受影响行数
        /// </summary>
        /// <param name="sql">The SQL statement to execute</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL</param>
        /// <returns>受影响行数</returns>
        int Execute(string sql, params object[] args);

        /// <summary>
        /// 执行sql 返回 受影响行数
        /// </summary>
        /// <param name="sql">An SQL builder object representing the query and it's arguments</param>
        /// <returns>受影响行数</returns>
        int Execute(Sql sql);

        #endregion

        #region operation: ExecuteScalar

        /// <summary>
        /// 执行查询，并返回结果集的第一行的第一列。
        /// </summary>
        /// <typeparam name="T">The type that the result value should be cast to</typeparam>
        /// <param name="sql">The SQL query to execute</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL</param>
        /// <returns>The scalar value cast to T</returns>
        T ExecuteScalar(string sql, params object[] args);

        /// <summary>
        /// 执行查询，并返回结果集的第一行的第一列。
        /// </summary>
        /// <typeparam name="T">The type that the result value should be cast to</typeparam>
        /// <param name="sql">An SQL builder object representing the query and it's arguments</param>
        /// <returns>The scalar value cast to T</returns>
        T ExecuteScalar(Sql sql);

        #endregion

        #region operation: Fetch


        /// <summary>
        /// 条件查询列表 where 后面的条件
        /// </summary>
        /// <param name="whereStr">条件</param>
        /// <returns></returns>
        List<T> FetchW(string whereStr);

        /// <summary>
        /// 条件查询列表 完整的sql
        /// </summary>
        /// <typeparam name="T2">类型</typeparam>
        /// <param name="sql">完整的sql</param>
        /// <returns></returns>
        List<T2> Fetch<T2>(string sql);
        /// <summary>
        ///sql查询列表
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">An SQL builder object representing the query and it's arguments</param>
        /// <returns>A List holding the results of the query</returns>
        List<T> Fetch(Sql sql);

        /// <summary>
        /// 条件查询列表 where 后面的条件
        /// </summary>
        /// <typeparam name="T2">类型</typeparam>
        /// <param name="whereStr">wehere后面的条件</param>
        /// <returns></returns>
        List<T2> FetchW<T2>(string whereStr);

        #endregion

        #region operation: Page




        /// <summary>
        /// sql检索一页记录，可用总记录数
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="page">The 1 based page number to retrieve</param>
        /// <param name="itemsPerPage">每页记录的数量</param>
        /// <param name="sql">An SQL builder object representing the base SQL query and it's arguments</param>
        /// <returns>A Page of results</returns>
        /// <remarks>
        /// PetaPoco will automatically modify the supplied SELECT statement to only retrieve the
        /// records for the specified page.  It will also execute a second query to retrieve the
        /// total number of records in the result set.
        /// </remarks>
        Page<T> Page(long page, long itemsPerPage, Sql sql);

        /// <summary>
        /// 条件翻页 where 后面的条件 
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <param name="whereStr">条件</param>
        /// <returns></returns>
        Page<T> PageW(long page, long itemsPerPage, string whereStr);
        #endregion

        #region operation: Fetch (page)

        /// <summary>
        /// sql 分页没有总数返回 返回列表
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="page">The 1 based page number to retrieve</param>
        /// <param name="itemsPerPage">每页记录的数量</param>
        /// <param name="sql">The base SQL query</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>A List of results</returns>
        /// <remarks>
        /// PetaPoco will automatically modify the supplied SELECT statement to only retrieve the
        /// records for the specified page.
        /// </remarks>
        List<T> Fetch(long page, long itemsPerPage, string sql, params object[] args);
        /// <summary>
        /// sql 分页没有总数返回 返回列表
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="page">The 1 based page number to retrieve</param>
        /// <param name="itemsPerPage">每页记录的数量</param>
        /// <param name="sql">An SQL builder object representing the base SQL query and it's arguments</param>
        /// <returns>A List of results</returns>
        /// <remarks>
        /// PetaPoco will automatically modify the supplied SELECT statement to only retrieve the
        /// records for the specified page.
        /// </remarks>
        List<T> Fetch(long page, long itemsPerPage, Sql sql);


        /// <summary>
        /// where后面条件分页  没有总数返回 返回列表
        /// </summary>
        /// <param name="page">页码</param>
        /// <param name="itemsPerPage">每页数量</param>
        /// <param name="whereStr">条件</param>
        /// <returns></returns>
        List<T> FetchW(long page, long itemsPerPage, string whereStr);

        #endregion

        #region operation: SkipTake

        /// <summary>
        /// Retrieves a range of records from result set
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="skip">The number of rows at the start of the result set to skip over</param>
        /// <param name="take">The number of rows to retrieve</param>
        /// <param name="sql">The base SQL query</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>A List of results</returns>
        /// <remarks>
        /// PetaPoco will automatically modify the supplied SELECT statement to only retrieve the
        /// records for the specified range.
        /// </remarks>
        List<T> SkipTake(long skip, long take, string sql, params object[] args);

        /// <summary>
        /// Retrieves a range of records from result set
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="skip">The number of rows at the start of the result set to skip over</param>
        /// <param name="take">The number of rows to retrieve</param>
        /// <param name="sql">An SQL builder object representing the base SQL query and it's arguments</param>
        /// <returns>A List of results</returns>
        /// <remarks>
        /// PetaPoco will automatically modify the supplied SELECT statement to only retrieve the
        /// records for the specified range.
        /// </remarks>
        List<T> SkipTake(long skip, long take, Sql sql);
        #endregion

        #region operation: Query

        /// <summary>
        /// sql 返回集合 IEnumerable
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">The SQL query</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>An enumerable collection of result records</returns>
        /// <remarks>
        /// For some DB providers, care should be taken to not start a new Query before finishing with
        /// and disposing the previous one. In cases where this is an issue, consider using Fetch which
        /// returns the results as a List rather than an IEnumerable.
        /// </remarks>
        IEnumerable<T> Query(string sql, params object[] args);

        /// <summary>
        /// sql 返回集合 IEnumerable
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">An SQL builder object representing the base SQL query and it's arguments</param>
        /// <returns>An enumerable collection of result records</returns>
        /// <remarks>
        /// For some DB providers, care should be taken to not start a new Query before finishing with
        /// and disposing the previous one. In cases where this is an issue, consider using Fetch which
        /// returns the results as a List rather than an IEnumerable.
        /// </remarks>
        IEnumerable<T> Query(Sql sql);
        /// <summary>
        /// where后面条件 返回集合 IEnumerable
        /// </summary>
        /// <param name="whereStr">条件</param>
        /// <returns></returns>
        IEnumerable<T> QueryW(string whereStr);
        #endregion

        #region operation: Exists

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T">The Type representing the table being queried</typeparam>
        /// <param name="sqlCondition">The SQL expression to be tested for (ie: the WHERE expression)</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>True if a record matching the condition is found.</returns>
        bool Exists(string sqlCondition, params object[] args);

        /// <summary>
        /// id判断 是否存在
        /// </summary>
        /// <typeparam name="T">The Type representing the table being queried</typeparam>
        /// <param name="primaryKey">The primary key value to look for</param>
        /// <returns>True if a record with the specified primary key value exists.</returns>
        bool Exists(object primaryKey);

        #endregion

        #region operation: linq style (Exists, Single, SingleOrDefault etc...)

        /// <summary>
        /// 主键 查一个
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="primaryKey">The primary key value of the record to fetch</param>
        /// <returns>The single record matching the specified primary key value</returns>
        /// <remarks>
        /// If there are no records with the specified primary key value, default(T) (typically null) is returned
        /// </remarks>
        T SingleM(object primaryKey);

        /// <summary>
        /// 主键查一个，如果 没有，返回默认实例
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="primaryKey">The primary key value of the record to fetch</param>
        /// <returns>The single record matching the specified primary key value</returns>
        /// <remarks>
        /// If there are no records with the specified primary key value, default(T) (typically null) is returned.
        /// </remarks>
        T SingleOrDefault(object primaryKey);



        /// <summary>
        /// sql 查一个，如果 没有，返回默认实例
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">The SQL query</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>The single record matching the specified primary key value, or default(T) if no matching rows</returns>
        T SingleOrDefault(string sql, params object[] args);

        /// <summary>
        /// sql查询，该查询应始终返回至少一个返回
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">The SQL query</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>The first record in the result set</returns>
        T First(string sql, params object[] args);


        /// <summary>
        /// where后条件 返回第一个
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        T FirstW(string whereStr);
        /// <summary>
        /// 运行一个查询，并返回第一个记录，或者如果没有则 返回默认值
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">The SQL query</param>
        /// <param name="args">Arguments to any embedded parameters in the SQL statement</param>
        /// <returns>The first record in the result set, or default(T) if no matching rows</returns>
        T FirstOrDefault(string sql, params object[] args);



        /// <summary>
        /// where后条件 返回单个
        /// </summary>
        /// <param name="whereStr"></param>
        /// <returns></returns>
        T SingleW(string whereStr);

        /// <summary>
        /// sql 查一个，如果 没有，返回默认实例
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">An SQL builder object representing the query and it's arguments</param>
        /// <returns>The single record matching the specified primary key value, or default(T) if no matching rows</returns>
        T SingleOrDefault(Sql sql);

        /// <summary>
        /// sql查询，该查询应始终返回至少一个返回
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">An SQL builder object representing the query and it's arguments</param>
        /// <returns>The first record in the result set</returns>
        T First(Sql sql);
        /// <summary>
        /// 运行一个查询，并返回第一个记录，或者如果没有则 返回默认值
        /// </summary>
        /// <typeparam name="T">The Type representing a row in the result set</typeparam>
        /// <param name="sql">An SQL builder object representing the query and it's arguments</param>
        /// <returns>The first record in the result set, or default(T) if no matching rows</returns>
        T FirstOrDefault(Sql sql);
        #endregion

        #region operation: Insert

        /// <summary>
        ///添加 指定 表，主键
        /// </summary>
        /// <param name="tableName">The name of the table to insert into</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="poco">The POCO object that specifies the column values to be inserted</param>
        /// <returns>The auto allocated primary key of the new record</returns>
        object Insert(string tableName, string primaryKeyName, T poco);



        /// <summary>
        /// 添加， 指定主键， 表，是否自增
        /// </summary>
        /// <param name="tableName">The name of the table to insert into</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="autoIncrement">是否自增</param>
        /// <param name="poco">The POCO object that specifies the column values to be inserted</param>
        /// <returns>自动分配新记录的主键，或非自动增量表的空值</returns>
        /// <remarks>Inserts a poco into a table.  If the poco has a property with the same name 
        /// as the primary key the id of the new record is assigned to it.  Either way,
        /// the new id is returned.</remarks>
        object Insert(string tableName, string primaryKeyName, bool autoIncrement, T poco);

        /// <summary>
        /// Performs an SQL Insert
        /// </summary>
        /// <param name="poco">The POCO object that specifies the column values to be inserted</param>
        /// <returns>自动分配新记录的主键，或非自动增量表的空值</returns>
        /// <remarks>设置表的名称，它的主键和是否检索自动分配的主键  从 POCO 的属性</remarks>
        object Insert(T poco);

        #endregion

        #region operation: Update

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName">更新的表名</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <param name="primaryKeyValue">The primary key of the record to be updated</param>
        /// <returns>The number of affected records</returns>
        int Update(string tableName, string primaryKeyName, T poco, object primaryKeyValue);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName">更新的表名</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <param name="primaryKeyValue">The primary key of the record to be updated</param>
        /// <param name="columns">要更新的列的列名称，或空的所有</param>
        /// <returns>The number of affected rows</returns>
        int Update(string tableName, string primaryKeyName, T poco, object primaryKeyValue, IEnumerable<string> columns);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName">更新的表名</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <returns>The number of affected rows</returns>
        int Update(string tableName, string primaryKeyName, T poco);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="tableName">更新的表名</param>
        /// <param name="primaryKeyName">The name of the primary key column of the table</param>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <param name="columns">要更新的列的列名称，或空的所有</param>
        /// <returns>The number of affected rows</returns>
        int Update(string tableName, string primaryKeyName, T poco, IEnumerable<string> columns);

        /// <summary>
        /// Performs an SQL update
        /// </summary>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <param name="columns">要更新的列的列名称，或空的所有</param>
        /// <returns>The number of affected rows</returns>
        int Update(T poco, IEnumerable<string> columns);

        /// <summary>
        /// Performs an SQL update
        /// </summary>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <returns>The number of affected rows</returns>
        int Update(T poco);

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <param name="primaryKeyValue">The primary key of the record to be updated</param>
        /// <returns>The number of affected rows</returns>
        int Update(T poco, object primaryKeyValue);

        /// <summary>
        /// Performs an SQL update
        /// </summary>
        /// <param name="poco">The POCO object that specifies the column values to be updated</param>
        /// <param name="primaryKeyValue">The primary key of the record to be updated</param>
        /// <param name="columns">要更新的列的列名称，或空的所有</param>
        /// <returns>The number of affected rows</returns>
        int Update(T poco, object primaryKeyValue, IEnumerable<string> columns);



        /// <summary>
        /// Performs an SQL update
        /// </summary>
        /// <typeparam name="T">The POCO class who's attributes specify 更新的表名</typeparam>
        /// <param name="sql">An SQL builder object representing the SQL update and condition clause (ie: everything after "UPDATE tablename"</param>
        /// <returns>The number of affected rows</returns>
        int Update(Sql sql);

        /// <summary>
        ///  除 指定列外，全部更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="poco"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        int UpdateNoIn(T poco, IEnumerable<string> Notcolumns);
        #endregion

        #region operation: Delete

        /// <summary>
        /// Performs and SQL Delete
        /// </summary>
        /// <param name="tableName">The name of the table to delete from</param>
        /// <param name="primaryKeyName">The name of the primary key column</param>
        /// <param name="poco">The POCO object whose primary key value will be used to delete the row</param>
        /// <returns>The number of rows affected</returns>
        int Delete(string tableName, string primaryKeyName, T poco);



        /// <summary>
        /// Performs an SQL Delete
        /// </summary>
        /// <param name="poco">The POCO object specifying the table name and primary key value of the row to be deleted</param>
        /// <returns>The number of rows affected</returns>
        int Delete(T poco);

        /// <summary>
        /// 根据主键删除 实体需要指名主键
        /// </summary>
        /// <param name="pocoOrPrimaryKey">主键值</param>
        /// <returns></returns>
        int DeleteM(object pocoOrPrimaryKey);



        /// <summary>
        /// Performs an SQL Delete
        /// </summary>
        /// <typeparam name="T">The POCO class who's attributes specify the name of the table to delete from</typeparam>
        /// <param name="sql">An SQL builder object representing the SQL condition clause identifying the row to delete (ie: everything after "UPDATE tablename"</param>
        /// <returns>The number of affected rows</returns>
        int Delete(Sql sql);

        /// <summary>
        /// where 后面条件删除 
        /// </summary>
        /// <param name="whereStr">条件</param>
        /// <returns></returns>
        int DeleteW(string whereStr);



        /// <summary>
        ///   批量删除 id集 (逗号 "," 隔开  字符串类型 每个id加  "'"号 )
        /// </summary>
        /// <param name="IDSet">id集</param>
        /// <returns></returns>
        int DeleteBatch(string IDSet);
        #endregion


        #region operation: Save
        /// <summary>
        /// 保存，没有则添加，有则更新
        /// </summary>
        /// <param name="tableName">The name of the table to be updated</param>
        /// <param name="primaryKeyName">The name of the primary key column</param>
        /// <param name="poco">The POCO object to be saved</param>
        void Save(string tableName, string primaryKeyName, T poco);

        /// <summary>
        /// 保存，没有则添加，有则更新
        /// </summary>
        /// <param name="poco">The POCO object to be saved</param>
        void Save(T poco);
        #endregion


    }
}
