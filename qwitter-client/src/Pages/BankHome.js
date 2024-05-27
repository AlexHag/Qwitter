import { useAuth } from "../Auth/AuthProvider";
import { useNavigate } from "react-router-dom";
import { useBankAPI } from "../Api/BankAPI";
import { useEffect, useState } from "react";
import Loading from "../Components/Loading/Loading";
import PageHeader from "../Components/PageHeader";
import Modal from "../Components/Modal";
import OpenNewBankAccount from "../Components/Bank/OpenNewBankAccount";
import TransferMoney from "../Components/Bank/TransferMoney";
import "../Styles/BankHome.css";

function BankHome() {
  const auth = useAuth();
  const navigate = useNavigate();
  const bankAPI = useBankAPI();
  const [accounts, setAccounts] = useState([]);
  const [primaryAccount, setPrimaryAccount] = useState(null);
  const [isLoading, setIsLoading] = useState(true);
  const [showOpenBankAccountModal, setShowOpenBankAccountModal] = useState(false);
  const [showTransferMoneyModal, setShowTransferMoneyModal] = useState(false);

  useEffect(() => {
    setIsLoading(true);
    bankAPI.getAccounts()
      .then(p => {
        const primary = p.find(a => a.isPrimary);
        setPrimaryAccount(primary);
        const otherAccounts = p.filter(a => !a.isPrimary);
        console.log(otherAccounts);
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
    console.log([primaryAccount, ...accounts])
  }

  return (
    <>
      <PageHeader />
      <button onClick={doStuff}>Dostuff</button>
      <div className="bank-home">
        <h1>Overview</h1>

        <div className="bank-home-body">

        {isLoading ? <Loading /> :
          <div className="bank-accounts">
            <h2>Accounts</h2>
            {primaryAccount == undefined ?
              <div>
                <div className="flex-space-between">
                  <h3>You don't have any accounts</h3>
                  <button 
                  style={{ margin: "36px", padding: "8px 16px" }} 
                  className="qwitter-button" 
                  onClick={() => setShowOpenBankAccountModal(true)}>
                    Open Account
                  </button>
                </div>
                <Modal show={showOpenBankAccountModal} >
                  <OpenNewBankAccount onClose={() => setShowOpenBankAccountModal(false)} />
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
                    <button 
                    style={{ margin: "36px 0px 0px 36px", 
                    padding: "8px 16px" }} 
                    className="qwitter-button" 
                    onClick={() => setShowOpenBankAccountModal(true)}>
                      Open another account
                    </button>
                    <Modal show={showOpenBankAccountModal} >
                      <OpenNewBankAccount onClose={() => setShowOpenBankAccountModal(false)} />
                    </Modal>
                  </div>
              </>}

          </div>}

        <div className="bank-actions">
          <div className="flex-space-between">
          <button 
            style={{ margin: "36px 0px 0px 36px", padding: "8px 16px" }} 
            className="qwitter-button">
              Deposit
          </button>
          <button 
            style={{ margin: "36px 36px 0px 36px", padding: "8px 16px" }}
            className="qwitter-button"
            onClick={() => setShowTransferMoneyModal(true)}>
              Transfer
          </button>
          <Modal show={showTransferMoneyModal} >
            <TransferMoney onClose={() => setShowTransferMoneyModal(false)} accounts={[primaryAccount, ...accounts]} primaryAccount={primaryAccount} />
          </Modal>
          </div>
        </div>

        </div>
      </div>
    </>
  )
}

export default BankHome;