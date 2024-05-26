import { useAuth } from "../Auth/AuthProvider";
import { useNavigate } from "react-router-dom";
import { useBankAPI } from "../Api/BankAPI";
import { useEffect, useState } from "react";
import Loading from "../Components/Loading/Loading";
import PageHeader from "../Components/PageHeader";
import "../Styles/BankHome.css";

function BankHome() {
  const auth = useAuth();
  const navigate = useNavigate();
  const bankAPI = useBankAPI();
  const [accounts, setAccounts] = useState([]);
  const [primaryAccount, setPrimaryAccount] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    setIsLoading(true);
    bankAPI.getAccounts()
      .then(p => {
        const primary = p.find(a => a.isPrimary);
        setPrimaryAccount(primary);
        const otherAccounts = p.filter(a => !a.isPrimary);
        setAccounts(otherAccounts);
        setIsLoading(false);
      })
      .catch((error) => {
        setIsLoading(false);
        console.error(error);
        navigate("/login");
      });
  }, [])

  const doStuff = () => {
    console.log(accounts);
    console.log(primaryAccount);
  }

  return (
    <>
      <PageHeader />
      <button onClick={doStuff}>Dostuff</button>
      <div className="bank-home">
        <h1>Overview</h1>

        {isLoading ? <Loading /> : 
        <div className="bank-accounts">
          <h2>Accounts</h2>
          <h3>Primary Account</h3>
          
          <div className="primary-bank-account-header">
            <p>Name</p>
            <p>Account Number</p>
            <p>Balance</p>
          </div>
          <div className="primary-bank-account-body">
            <p>{primaryAccount.accountName}</p>
            <p>{primaryAccount.accountNumber}</p>
            <p>{primaryAccount.balance}</p>
          </div>

          <h3>Other Accounts</h3>
          <div className="primary-bank-account-header">
            <p>Name</p>
            <p>Account Number</p>
            <p>Balance</p>
          </div>
          {accounts.map(account => 
          <div className="primary-bank-account-body">
            <p>{account.accountName}</p>
            <p>{account.accountNumber}</p>
            <p>{account.balance}</p>
          </div>)}          
        </div>}

        <div className="bank-actions">
          <button>Open new account</button>
          <button>Deposit</button>
          <button>Send money</button>
        </div>
      </div>
    </>
  )
}

export default BankHome;