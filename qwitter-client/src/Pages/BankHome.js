import { useNavigate } from "react-router-dom";
import { useBankAPI } from "../Api/BankAPI";
import { useEffect, useState } from "react";
import PageHeader from "../Components/PageHeader";
import TransferMoneyModal from "../Components/Bank/TransferMoneyModal";
import AccountsView from "../Components/Bank/AccountsView";
import DepositMoneyModal from "../Components/Bank/DepositMoneyModal";
import "../Styles/BankHome.css";

function BankHome() {
  const navigate = useNavigate();
  const bankAPI = useBankAPI();
  const [accounts, setAccounts] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const [showTransferMoneyModal, setShowTransferMoneyModal] = useState(false);
  const [showDepositMoneyModal, setShowDepositMoneyModal] = useState(false);

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
      <div className="bank-home">
        <h1>Overview</h1>
        <div className="bank-home-body">
          <AccountsView accounts={accounts} fetchAccounts={fetchAccounts} isLoading={isLoading} />
          <div className="bank-actions">
            <div className="flex-space-between">
              <button
                style={{ margin: "36px 0px 0px 36px", padding: "8px 16px" }}
                className="qwitter-button"
                // onClick={() => navigate("/deposit")}>
                onClick={() => setShowDepositMoneyModal(true)}>
                Deposit
              </button>
              <DepositMoneyModal show={showDepositMoneyModal} onClose={() => setShowDepositMoneyModal(false)} accounts={accounts} callback={fetchAccounts} />
              <button
                style={{ margin: "36px 36px 0px 36px", padding: "8px 16px" }}
                className="qwitter-button"
                onClick={() => setShowTransferMoneyModal(true)}>
                Transfer
              </button>
              <TransferMoneyModal show={showTransferMoneyModal} onClose={() => setShowTransferMoneyModal(false)} accounts={accounts} callback={fetchAccounts} />
            </div>
          </div>
        </div>
      </div>
    </>
  )
}

export default BankHome;