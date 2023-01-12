import Button from 'react-bootstrap/Button';
import Modal from 'react-bootstrap/Modal';
import React, { useState } from 'react';
import FloatingLabel from 'react-bootstrap/FloatingLabel';
import Form from 'react-bootstrap/Form';
import axios from 'axios';

const BugForm = (props) => {
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');

  const CreateBug = () => {
    console.log('Create bug');
    console.log(title);
    console.log(description);
    if (title !== '' && description !== '') {
      var body = {
        title: title,
        description: description,
      };
      axios.post('https://localhost:7194/api/v1/bug', body).then((resp) => {
        console.log(resp);
      });
    }

    props.onHide();
    setTitle('');
    setDescription('');
  };

  return (
    <Modal {...props} size="lg" aria-labelledby="contained-modal-title-vcenter" centered>
      <Modal.Header closeButton>
        <Modal.Title id="contained-modal-title-vcenter">Add Bug</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        <FloatingLabel controlId="title" label="Title" className="mb-3">
          <Form.Control
            type="text"
            placeholder="Title"
            onChange={(e) => {
              setTitle(e.target.value);
            }}
          />
        </FloatingLabel>
        <FloatingLabel controlId="description" label="Description" className="mb-3">
          <Form.Control
            type="text"
            placeholder="Description"
            onChange={(e) => {
              setDescription(e.target.value);
            }}
          />
        </FloatingLabel>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={props.onHide}>
          Close
        </Button>
        <Button variant="success" onClick={CreateBug}>
          Create
        </Button>
      </Modal.Footer>
    </Modal>
  );
};
export default BugForm;
