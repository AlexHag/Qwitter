import { useBankAPI } from "../../Api/BankAPI";
import { useState } from "react";
import "../../Styles/BankHome.css";

function OpenNewBankAccountModal({ show, onClose, callback }) {
  const [accountName, setAccountName] = useState(null);
  const [currency, setCurrency] = useState("USD");
  const [accountType, setAccountType] = useState("Debit");
  const [error, setError] = useState(null);
  const bankAPI = useBankAPI();

  if (!show) {
    return null;
  }

  const handleSubmit = () => {
    if (accountName == null || accountName == "") {
      setError("Account name can't be empty");
      return;
    }
    bankAPI.createBankAccount(accountName, currency, accountType)
      .then(() => {
        setError(null);
        onClose();
        callback();
      })
      .catch((error) => {
        console.error(error);
        error.json().then(e => setError(e.error))
      });
  }

  return (
    <div className="default-modal">
      <div className="default-modal-sub">
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
      </div>
    </div>
  )
}

export default OpenNewBankAccountModal;