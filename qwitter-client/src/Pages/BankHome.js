import { useAuth } from "../Auth/AuthProvider";
import { useNavigate } from "react-router-dom";
import { useBankAPI } from "../Api/BankAPI";
import { useEffect, useState } from "react";
import Loading from "../Components/Loading/Loading";
import PageHeader from "../Components/PageHeader";
import Modal from "../Components/Modal";
import "../Styles/BankHome.css";

function OpenNewBankAccount({ onClose }) {
  const auth = useAuth();
  const [accountName, setAccountName] = useState(null);
  const [currency, setCurrency] = useState("USD");
  const [accountType, setAccountType] = useState("Debit");
  const [error, setError] = useState(null);
  const bankAPI = useBankAPI();

  const handleSubmit = () => {
    if (accountName == null || accountName == "") {
      setError("Account name can't be empty");
      return;
    }
    bankAPI.createBankAccount(accountName, currency, accountType)
      .then(() => {
        setError(null);
        onClose();
        window.location.reload();
      })
      .catch((error) => {
        console.error(error);
        setError("Something went wrong! Try again later");
      });
  }

  return (
    <>
      <div style={{ paddingBottom: "36px" }} className="flex-space-between">
        <h1>Open new bank account</h1>
        <button className="qwitter-button-gray" onClick={onClose}>Close</button>
      </div>

      <div onSubmit={handleSubmit} className="auth-form-input">
        <label htmlFor="account-name">Account Name</label>
        <input
          id="account-name"
          name="accountName"
          placeholder="Account Name"
          onChange={(e) => setAccountName(e.target.value)}
        />

        <label style={{ marginTop: "18px" }} htmlFor="currency">Currency</label>
        <select className="custom-select" name="currency" id="currency" onChange={(e) => setCurrency(e.target.value)}>
          <option value="USD">USD</option>
          <option value="SEK">SEK</option>
          <option value="EUR">EUR</option>
          <option value="BTC">BTC</option>
          <option value="ETH">ETH</option>
        </select>

        <label style={{ marginTop: "18px" }} htmlFor="accountType">Type</label>
        <select name="accountType" id="accountType" onChange={(e) => setAccountType(e.target.value)}>
          <option value="Debit">Debit</option>
          <option value="Credit">Credit</option>
        </select>

        <br></br>

        <p style={{ color: "red", marginTop: "18px" }}>{error}</p>

        <button style={{ marginTop: "18px" }} className="qwitter-button" onClick={handleSubmit}>Submit</button>
      </div>
    </>
  )
}

function BankHome() {
  const auth = useAuth();
  const navigate = useNavigate();
  const bankAPI = useBankAPI();
  const [accounts, setAccounts] = useState([]);
  const [primaryAccount, setPrimaryAccount] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  const [showOpenBankAccountModal, setShowOpenBankAccountModal] = useState(false);

  const handleOpenBankAccountModal = () => {
    setShowOpenBankAccountModal(true);
  };

  const handleCloseOpenBankAccountModal = () => {
    setShowOpenBankAccountModal(false);
  };

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
            {primaryAccount == undefined ?
              <div>
                <div className="flex-space-between">
                  <h3>You don't have any accounts</h3>
                  <button style={{ margin: "36px", padding: "8px 16px" }} className="qwitter-button" onClick={handleOpenBankAccountModal}>Open Account</button>
                </div>
                <Modal show={showOpenBankAccountModal} >
                  <OpenNewBankAccount onClose={handleCloseOpenBankAccountModal} />
                </Modal>
              </div>
              :
              <>
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

                {accounts == undefined || accounts.length == 0 ?
                  null :
                  <>
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
                  </>}
                  <div>
                    <button style={{ margin: "36px 0px 0px 36px", padding: "8px 16px" }} className="qwitter-button" onClick={handleOpenBankAccountModal}>Open another account</button>
                    <Modal show={showOpenBankAccountModal} >
                      <OpenNewBankAccount onClose={handleCloseOpenBankAccountModal} />
                    </Modal>
                  </div>
              </>}

          </div>}

        <div className="bank-actions">
          <button>Deposit</button>
          <button>Send money</button>
        </div>
      </div>
    </>
  )
}

export default BankHome;