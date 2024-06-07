import { useState } from "react";
import Loading from "../../Components/Loading/Loading";
import OpenNewBankAccountModal from "./OpenNewBankAccountModal";
import "../../Styles/BankHome.css";

function AccountsView({ accounts, fetchAccounts, isLoading }) {
  const [showOpenBankAccountModal, setShowOpenBankAccountModal] = useState(false);

  return (
    <>
      {isLoading ? <Loading /> :
        <div className="bank-accounts">
          <h2>Accounts</h2>
          {accounts[0] == undefined ?
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
              <OpenNewBankAccountModal show={showOpenBankAccountModal} onClose={() => setShowOpenBankAccountModal(false)} callback={fetchAccounts} />
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
                <p>{accounts[0].accountName}</p>
                <p>{accounts[0].accountNumber}</p>
                <p>{accounts[0].balance}</p>
              </div>

              {accounts.length < 1 ?
                null :
                <>
                  <h3>Other Accounts</h3>
                  <div className="primary-bank-account-header">
                    <p>Name</p>
                    <p>Account Number</p>
                    <p>Balance</p>
                  </div>
                  {accounts.slice(1).map(account =>
                    <div className="primary-bank-account-body">
                      <p>{account.accountName}</p>
                      <p>{account.accountNumber}</p>
                      <p>{account.balance}</p>
                    </div>)}
                </>}
              <div>
                <button
                  style={{
                    margin: "36px 0px 0px 36px",
                    padding: "8px 16px"
                  }}
                  className="qwitter-button"
                  onClick={() => setShowOpenBankAccountModal(true)}>
                  Open another account
                </button>
                <OpenNewBankAccountModal show={showOpenBankAccountModal} onClose={() => setShowOpenBankAccountModal(false)} callback={fetchAccounts} />
              </div>
            </>}

        </div>}
    </>
  );
}

export default AccountsView;