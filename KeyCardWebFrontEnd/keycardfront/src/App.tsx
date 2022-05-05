import axios from "axios";
import {
  BrowserRouter as Router,
  Routes,
  Route,
  Link
} from "react-router-dom";
import Home from "./component/Home";
import Login from "./component/Login";

export default function App(): JSX.Element {
  axios.defaults.baseURL = process.env.REACT_APP_BACKEND;

  return (
    <Router>
      <div>
        <nav>
          <ul>
            <li>
              <Link to="/">Home</Link>
            </li>
            <li>
              <Link to="/login">Login</Link>
            </li>
          </ul>
        </nav>

        <Routes>
          <Route path="/" element={< Home />}></Route>
          <Route path="/login" element={< Login />} ></Route>
        </Routes>

      </div>
    </Router>
  );
}
