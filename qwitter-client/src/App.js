import './App.css';
import { BrowserRouter as Router, Route, Routes } from "react-router-dom";
import Login from './Pages/Login';
import Register from './Pages/Register';
import Profile from './Pages/Profile';
import AuthProvider from './Auth/AuthProvider';
import PrivateRoute from './Auth/PrivateRoute';
import BankHome from './Pages/BankHome';
import Home from './Pages/Home';

function App() {
  return (
    <div className="App">
      <Router>
        <AuthProvider>
          <Routes>
          <Route path="/login" element={<Login />} />
          <Route path="/register" element={<Register />} />
            <Route element={<PrivateRoute />}>
              {/* Private routes that require authentication */}
              <Route path="/profile" element={<Profile />} />
              <Route path="/bank" element={<BankHome />} />
            </Route>
            <Route path="/" element={<Home />} />
          </Routes>
        </AuthProvider>
      </Router>
    </div>
  );
}

export default App;
