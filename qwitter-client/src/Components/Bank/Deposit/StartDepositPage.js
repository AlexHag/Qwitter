import { useNavigate } from "react-router-dom";
import { useBankAPI } from "../../../Api/BankAPI";
import { useEffect, useState } from "react";
import Loading from "../../Loading/Loading";
import PageHeader from "../../PageHeader";
import "./deposit.css";

function StartDepositPage() {
  const navigate = useNavigate();
  const bankAPI = useBankAPI();
  const [accounts, setAccounts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [selectedAccount, setSelectedAccount] = useState(0);
  const [currency, setCurrency] = useState("USD");

  useEffect(() => {
    fetchAccounts();
  }, [])

  const fetchAccounts = () => {
    setIsLoading(true);
    bankAPI.getAccounts()
      .then(p => {
        setAccounts(p.sort(p => p.isPrimary ? -1 : 1));
        setIsLoading(false);
      })
      .catch((error) => {
        setIsLoading(false);
        console.error(error);
        navigate("/login");
      });
  }

  return (
    <>
      <PageHeader />
      <div className="start-deposit-form">
      <button className="qwitter-button" style={{marginBottom: "80px"}} onClick={() => navigate("/bank")}>
        Back to bank
      </button>
      {isLoading ? <Loading /> :
        <>
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

          <button className="qwitter-button" style={{marginTop: "18px", display: "block"}} onClick={() => navigate("/bank")}>
            Continue
          </button>
        </>
      }
      </div>
    </>
  );
}

export default StartDepositPage;