import './App.css';
import Header from './Modules/Header.jsx';
import Editor from './Modules/Editor.jsx';
import Sidebar from './Modules/Sidebar.jsx';

function App() {

  return (
    <>
      <div className="grid grid-cols-[1fr_3fr] grid-rows-[auto_1fr] h-screen">
        <Header />
        <Sidebar />
        <Editor />
      </div>

    </>
  )
}

export default App
