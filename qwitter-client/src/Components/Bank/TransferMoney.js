import { useAuth } from "../../Auth/AuthProvider";
import { useBankAPI } from "../../Api/BankAPI";
import { useState } from "react";

function TransferMoney({ onClose, accounts }) {
  const auth = useAuth();
  const bankAPI = useBankAPI();
  const [amount, setAmount] = useState(null);
  const [fromAccountIdx, setFromAccountIdx] = useState(0);
  const [toAccountIdx, setToAccountIdx] = useState(0);
  const [error, setError] = useState(null);

  const handleSubmit = () => {
    if (fromAccountIdx == toAccountIdx) {
      setError("Can't transfer to the same account");
      return;
    }
    
    const fromAccount = accounts[fromAccountIdx];
    const toAccount = accounts[toAccountIdx];

    console.log(fromAccount);
    console.log(toAccount);

    if (amount == null || amount == "" || amount <= 0) {
      setError("Amount must be greater than 0");
      return;
    }

    if (isNaN(amount)) {
      setError("Please enter a valid amount");
      return;
    }

    if (fromAccount.balance < amount) {
      setError("Insufficient funds");
      return;
    }

    bankAPI.internalBankTransfer(fromAccount.id, toAccount.id, amount)
      .then(() => {
        setError(null);
        onClose();
        window.location.reload();
      })
      .catch((error) => {
        console.error(error);
        error.json().then(e => setError(e.error))
      });
  }

  return (
    <>
      <div style={{ paddingBottom: "36px" }} className="flex-space-between">
        <h1>Transfer</h1>
        <button className="qwitter-button-gray" onClick={onClose}>Close</button>
      </div>

      <div onSubmit={handleSubmit} className="auth-form-input">
        <label htmlFor="amount">Amount</label>
        <input
          id="amount"
          name="amount"
          placeholder="Amount"
          type="number"
          value={amount.toFixed(2)}
          onChange={(e) => setAmount(e.target.value)}
        />

        <div style={{ paddingTop: "18px" }} className="flex-space-between">
          <button className="qwitter-button-gray" style={{width: "15%"}} onClick={() => setAmount(10)}>10</button>
          <button className="qwitter-button-gray" style={{width: "15%"}} onClick={() => setAmount(25)}>25</button>
          <button className="qwitter-button-gray" style={{width: "15%"}} onClick={() => setAmount(50)}>50</button>
          <button className="qwitter-button-gray" style={{width: "15%"}} onClick={() => setAmount(100)}>100</button>
          <button className="qwitter-button-gray" style={{width: "15%"}} onClick={() => setAmount(accounts[fromAccountIdx].balance)}>Max ({accounts[fromAccountIdx].balance.toFixed(2)})</button>
        </div>
        
        <label style={{ marginTop: "18px" }} htmlFor="fromAccount">From</label>
        <select name="fromAccount" id="fromAccount" onChange={(e) => setFromAccountIdx(e.target.value)}>
          {accounts.map((account, index) => <option key={index} value={index}>{account.accountName}</option>)}
        </select>
        <p style={{ marginTop: "18px" }}>Avaliable balance: {accounts[fromAccountIdx].balance} {accounts[fromAccountIdx].currency}</p>

        <label style={{ marginTop: "18px" }} htmlFor="toAccount">From</label>
        <select name="toAccount" id="toAccount" onChange={(e) => setToAccountIdx(e.target.value)}>
          {accounts.map((account, index) => <option key={index} value={index}>{account.accountName}</option>)}
        </select>
        <p style={{ marginTop: "18px" }}>Avaliable balance: {accounts[toAccountIdx].balance} {accounts[toAccountIdx].currency}</p>

        <p style={{ color: "red", marginTop: "18px" }}>{error}</p>

        <button style={{ marginTop: "18px" }} className="qwitter-button" onClick={handleSubmit}>Transfer</button>

      </div>
    </>
  )
}

export default TransferMoney;