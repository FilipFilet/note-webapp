import './App.css';
import ContentPage from './Pages/ContentPage';
import LoginPage from './Pages/LoginPage.jsx';
import { Route, Routes } from 'react-router';
import ProtectedRoute from './ProtectedRoute.jsx';
import PublicRoute from './PublicRoute.jsx';

function App() {

  return (
    <Routes>
      <Route path="/" element={
        <LoginPage />
      } />
      <Route path="/Content" element={

        <ContentPage />
      } />
      {/* <Route path="*" element={<UnknownPage/>} /> */}
    </Routes>

  )
}

export default App
