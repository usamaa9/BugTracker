import axios from 'axios';
import { useState, useEffect } from 'react';
import Loading from './components/Loading';
import BugList from './components/BugList';
import Header from './components/Header';
import Footer from './components/Footer';
import Button from 'react-bootstrap/Button';
import BugForm from './components/BugForm';

const App = () => {
  useEffect(() => {
    axios.get('https://localhost:7194/api/v1/bug/list?status=all').then((result) => {
      setBugs(result.data);
    });
    document.title = 'Bug Tracker';
  }, []);

  const [bugs, setBugs] = useState(null);
  const [formShow, setFormShow] = useState(false);

  return (
    <div>
      <Header />
      <h2>List of bugs</h2>
      <Button variant="success" onClick={() => setFormShow(true)}>
        Add
      </Button>
      <BugForm show={formShow} onHide={() => setFormShow(false)} />
      {bugs ? <BugList bugs={bugs} /> : <Loading />}
      <Footer />
    </div>
  );
};

export default App;
