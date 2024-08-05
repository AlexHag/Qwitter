import { useState } from "react";
import { useBankAPI } from "../../Api/BankAPI";
import { useNavigate } from "react-router-dom";

function DepositMoneyModal({ show, onClose, accounts, callback }) {
  const bankAPI = useBankAPI();
  const navigate = useNavigate();
  const [selectedAccount, setSelectedAccount] = useState(0);
  const [currency, setCurrency] = useState("USD");

  if (!show) {
    return null;
  }

  const handleSubmit = () => {
    if (currency === "ETH") {
      navigate("/crypto-deposit");
    }
    // const account = accounts[selectedAccount];
    // console.log(account);
    // console.log(currency);
    // bankAPI.getCryptoWallet(account.id, currency).then(p => console.log(p));
  }

  return (
    <div className="default-modal">
      <div className="default-modal-sub">
        <div style={{ paddingBottom: "36px" }} className="flex-space-between">
          <h1>Deposit</h1>
          <button className="qwitter-button-gray" onClick={onClose}>Close</button>
        </div>

        <div className="auth-form-input">

        <label htmlFor="fromAccount">Select Account</label>
        <select name="fromAccount" id="fromAccount" onChange={(e) => setSelectedAccount(e.target.value)}>
          {accounts.map((account, index) => <option key={index} value={index}>{account.accountName}</option>)}
        </select>


        <label style={{ marginTop: "18px" }} htmlFor="currency">Select Currency</label>
        <select className="custom-select" name="currency" id="currency" onChange={(e) => setCurrency(e.target.value)}>
          <option value="USD">USD</option>
          <option value="SEK">SEK</option>
          <option value="EUR">EUR</option>
          <option value="BTC">BTC</option>
          <option value="ETH">ETH</option>
        </select>

        <br></br>
        <button style={{ marginTop: "18px" }} className="qwitter-button" onClick={handleSubmit}>Continue</button>

        </div>
      </div>
    </div>
  )
}

export default DepositMoneyModal;