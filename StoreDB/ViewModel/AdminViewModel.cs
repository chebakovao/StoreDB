using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Contexts;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Xml;
using Microsoft.EntityFrameworkCore;
using OnlineStoreDB.Model;

namespace OnlineStoreDB.ViewModel
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private List<string> _tableNames;
        public List<string> TableNames
        {
            get { return _tableNames; }
            set
            {
                _tableNames = value;
                OnPropertyChanged(nameof(TableNames));
            }
        }

        private string _selectedTable;
        public string SelectedTable
        {
            get { return _selectedTable; }
            set
            {
                _selectedTable = value;
                OnPropertyChanged(nameof(SelectedTable));
                
                LoadTableData();
                ApplyFilter();

            }
        }

        public AdminViewModel()
        {

            LoadTableNames();
            SaveChangesCommand = new RelayCommand(SaveChanges);
            AddNewRowCommand = new RelayCommand(AddNewRow);
            DeleteSelectedRowsCommand = new RelayCommand(DeleteSelectedRows);

        }

        private void LoadTableNames()
        {
            
            PropertyInfo[] dbSetProperties = typeof(ApplicationContextDB).GetProperties()
                                            .Where(p => p.PropertyType.IsGenericType &&
                                                        p.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
                                            .ToArray();

            
            TableNames = dbSetProperties.Select(p => p.Name).ToList();
        }

        private ObservableCollection<object> _tableData;
        public ObservableCollection<object> TableData
        {
            get { return _tableData; }
            set
            {
                _tableData = value;
                OnPropertyChanged(nameof(TableData));
            }
        }
        private void LoadTableData()
        {
            if (string.IsNullOrEmpty(SelectedTable))
            {
                TableData = null; 
                return;
            }

            using (var context = new ApplicationContextDB())
            {
            
                var dbSetProperty = typeof(ApplicationContextDB).GetProperty(SelectedTable);

                if (dbSetProperty != null)
                {
                    
                    var entityType = dbSetProperty.PropertyType.GetGenericArguments().First();

                   
                    var dbSet = context.GetType().GetMethod("Set").MakeGenericMethod(entityType).Invoke(context, null);

                    
                    var tableData = ((IEnumerable<object>)dbSet).ToList();
                    TableData = new ObservableCollection<object>(tableData);
                }
                else
                {
                    TableData = null; 
                }
            }
        }

        public ICommand SaveChangesCommand {  get; set; }
        private void SaveChanges(object obj)
        {
            if (TableData == null)
            {
                
                return;
            }

            
            using (var context = new ApplicationContextDB())
            {
                
                var entityType = typeof(ApplicationContextDB).Assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == SelectedTable);

                if (entityType == null)
                {
                    
                    return;
                }

                
                var dbSet = context.GetType().GetMethod("Set").MakeGenericMethod(entityType).Invoke(context, null);

                
                foreach (var entity in TableData)
                {
                    context.Entry(entity).State = EntityState.Modified;
                }

                
                context.SaveChanges();
            }
            ApplyFilter();
        }

       

        public ICommand DeleteSelectedRowsCommand {  get; set; }
        public ICommand AddNewRowCommand {  get; private set; }
       

        private void AddNewRow(object obj)
        {
            if (string.IsNullOrEmpty(SelectedTable))
            {
                
                return;
            }

            using (var context = new ApplicationContextDB())
            {
                
                var entityType = typeof(ApplicationContextDB).Assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == SelectedTable);

                if (entityType == null)
                {
                    
                    return;
                }

               
                var newEntity = Activator.CreateInstance(entityType);

               
                context.Add(newEntity);

                
                context.SaveChanges();

               
                LoadTableData();
                ApplyFilter();
            }
        }
        public IEnumerable<object> selectedRows {  get; set; }

        private void DeleteSelectedRows(object obj)
        {
            if (string.IsNullOrEmpty(SelectedTable) || TableData == null || !TableData.Any())
            {
                return;
            }

            if (selectedRows == null || !selectedRows.Any())
            {
                return;
            }

            using (var context = new ApplicationContextDB())
            {
                var entityType = typeof(ApplicationContextDB).Assembly.GetTypes()
                    .FirstOrDefault(t => t.Name == SelectedTable);

                if (entityType == null)
                {
                    return;
                }

                var dbSet = context.GetType().GetMethod("Set").MakeGenericMethod(entityType).Invoke(context, null);

                foreach (var selectedRow in selectedRows)
                {
                    if (context.Entry(selectedRow).State == EntityState.Detached)
                    {
                        dbSet.GetType().GetMethod("Attach").Invoke(dbSet, new[] { selectedRow });
                    }

                    dbSet.GetType().GetMethod("Remove").Invoke(dbSet, new[] { selectedRow });
                }

                context.SaveChanges();

                LoadTableData(); 
                ApplyFilter();
            }
        }


        private string _filterText;
        public string FilterText
        {
            get { return _filterText; }
            set
            {
                _filterText = value;
                OnPropertyChanged(nameof(FilterText));
                ApplyFilter();
            }
        }

        private ObservableCollection<object> _filteredTableData;
        public ObservableCollection<object> FilteredTableData
        {
            get { return _filteredTableData; }
            set
            {
                _filteredTableData = value;
                OnPropertyChanged(nameof(FilteredTableData));
            }
        }

        private void ApplyFilter()
        {
            if (string.IsNullOrEmpty(FilterText))
            {
                FilteredTableData = TableData; 
            }
            else
            {
                FilteredTableData = new ObservableCollection<object>();
                if (SelectedTable != null)
                {
                    foreach (var item in TableData)
                    {
                        var properties = item.GetType().GetProperties();
                        foreach (var property in properties)
                        {
                            var value = property.GetValue(item)?.ToString();
                            if (!string.IsNullOrEmpty(value) && value.Contains(FilterText))
                            {
                                FilteredTableData.Add(item);
                                break;
                            }
                        }
                    }
                }
                else return;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}