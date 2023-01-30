using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using CovalentSDK.Covalent;
using ReactiveUI;

namespace TransactionMonitor.ViewModels;

public class ProfileViewModel : ViewModelBase
{
    public ProfileViewModel()
    {
        LoadDataTransactions = ReactiveCommand.Create(() =>
        {
            try
            {
                var covalentMethods = new CovalentMethods();

                var TransactionHash = covalentMethods.GetATransaction(ChainId, Hash);

                var hash = " ";
                var status = " ";
                var valuetransactions = " ";

                var TransactionSelect =
                    from transaction in TransactionHash["data"]["items"]
                    select new
                    {
                        hash = (string) transaction["tx_hash"],
                        status = (string) transaction["successful"],
                        valuetransactions = (string) transaction["value"]
                    };

                foreach (var value in TransactionSelect)
                    TransactionsList.Add(new Transaction
                    {
                        Hash = value.hash,
                        Successful = value.status,
                        ValueTransaction = value.valuetransactions
                    });
            }
            catch (Exception e)
            {
            }
        });
    }

    public ObservableCollection<TransactionWithProtocol> TransactionWithProtocolsList { get; } = new();
    public ObservableCollection<Transaction> TransactionsList { get; } = new();

    public string ChainId { get; set; }
    public string DexName { get; set; }
    public string Wallet { get; set; }
    public string Hash { get; set; }

    public ICommand LoadDataTransactions { get; }

    public void LoadCollection()
    {
        try
        {
            var covalentMethods = new CovalentMethods();
            var service = new Service();

            var TransactionsProtoc = covalentMethods.GetXyTransaction(ChainId, DexName, Wallet);

            var hash = " ";
            var act = " ";
            var quote_currensy = " ";
            var total_quote = " ";

            var Transaction =
                from transaction in TransactionsProtoc["data"]["items"]
                select new
                {
                    hash = (string) transaction["tx_hash"],
                    act = (string) transaction["act"],
                    quote_currensy = (string) transaction["quote_currency"],
                    total_quote = (string) transaction["total_quote"]
                };
            foreach (var value in Transaction)
            {
                TransactionWithProtocolsList.Add(new TransactionWithProtocol
                {
                    Hash = value.hash, Act = value.act,
                    Quote_Currensy = value.quote_currensy,
                    Total_Quote = value.total_quote
                });
                OnPropertyChanged();
            }
        }
        catch (Exception e)
        {
        }
    }

    public void LoadData(string id, string dex, string wallet)
    {
        ChainId = id;
        DexName = dex;
        Wallet = wallet;
    }
}