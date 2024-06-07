import { useAuth } from "../Auth/AuthProvider";
import { useNavigate } from "react-router-dom";
import { useState, useEffect } from "react";
import PageHeader from "../Components/PageHeader";
import '../Styles/HomePage.css';

function Home() {
  const auth = useAuth();
  const [mainButtonText, setMainButtonText] = useState("Get Started");
  const navigate = useNavigate();

  useEffect(() => {
    if (auth.user) setMainButtonText("Go to Bank");
  }, [auth.token]);

  const handleOnMainButtonClick = () => {
    if (auth.token) {
      navigate("/bank");
    } else {
      navigate("/login");
    }
  }

  return (
    <div className="landing-page">
      <PageHeader />
      <div className="line" id="Home">
        <div className="line-side1">
          <h1>The bank you probably don't need</h1>
          <p style={{display: auth.user ? '' : 'none'}}>Welcome {auth?.user?.username}</p>
          <button className="qwitter-button" onClick={handleOnMainButtonClick}>
            {mainButtonText}
          </button>
        </div>
        <div className="line-side2">
          <img src="wide_bank_logo.png" width="500" />
        </div>
      </div>
      <section className="about" id="My Projects">
        <div className="content">
          <div className="title">
            <span>Our Services</span>
          </div>
          <div className="boxes">
            <div className="box">
              <div className="topic">
                <a href="" target="_blank">
                  Online Banking
                </a>
              </div>
              <p>
                Manage your accounts online, anytime, anywhere.
              </p>
            </div>
            <div className="box">
              <div className="topic">
                <a href="" target="_blank">
                  Mobile Banking
                </a>
              </div>
              <p>
                Bank on-the-go with our mobile app.
              </p>
            </div>
            <div className="box">
              <div className="topic">
                <a href="" target="_blank">
                  Loan Services
                </a>
              </div>
              <p>
                Get a loan that suits your needs.
              </p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}

export default Home;