import './App.css';
import ContentPage from './Pages/ContentPage';
import LoginPage from './Pages/LoginPage.jsx';
import { Route, Routes } from 'react-router';

function App() {

  return (
    <Routes>
      <Route path="/" element={<LoginPage />} />
      <Route path="/Content" element={<ContentPage />} />
      {/* <Route path="*" element={<UnknownPage/>} /> */}
    </Routes>
    
  )
}

export default App
