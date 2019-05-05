<template>
</template>
<style>
.content-input {
    font-size: 20px;
    margin-top: 10px;
    color: #3a3654;
    border-radius: 0px;
    box-shadow: 0px 1px 0px 0 #5d5d5d;
    border: 0;
    padding: 20px 0 10px 10px;
    width: 100%;
    font-family: "Montserrat", "Helevtica", sans-serif;
    color: #3a3654;
    margin-left: -5px;
    z-index: 2;
    background: transparent;
    transition: .3s;
}
.input-failed {
  box-shadow: 0 2px 0px 0px rgba(232, 76, 76, 0.8)
}
.input-group {
    width: 70%;
    margin: 15px auto;
    text-align: center;
    clear: both;
    position: relative;
}
.input-group label {
  display: block;
  text-align: left;
  margin-left: -5px;
  position: absolute;
  top: 10%;
}

.content-input:active,
.content-input:focus {
  box-shadow: 0px 0px 0px 2px #5d5d5d;
  margin-top: 30px;
  padding: 20px 0 20px 10px;
}

.content-input:active + label,
.content-input:focus + label {
    top: 0;
  
}

.radio-block {
    display: initial;
    padding: 15px;
    box-shadow: 0px 0px 0px 1px #5d5d5d45;
    position: relative;
    float: left;
    transition: .3s box-shadow;
    margin: 35px 15px 10px 0;
}
.radio-block:hover,
.radio-block.checked-radio {
  box-shadow: 0px 0px 0px 1px #5d5d5d;
}
.radio-block i {
  opacity: 0;
  width: 0;
  transition: .3s opacity;
}
.radio-block.checked-radio i {
  width: auto;
  opacity: 1;
}
.radio-label {
  margin-bottom: 15px;
}
.radio-item {
    position: absolute;
    width: 100%;
    height: 100%;
    left: -5px;
    cursor: pointer;
    top: -3px;
    opacity: 0;
}
form {
    width: 50%;
    margin: 20px auto;
    padding: 10px;
    border-radius: 5px;
    box-shadow: 0px 0px 20px 9px #cccccc59;
}
@media (max-width: 700px) {

  .content-input {
    padding: 10px 0px 10px 6px;
    font-size: 16px;
    margin-top: 20px;
    margin-left: -3px;
  }

  .input-group label {
    margin-left: -3px;
  }

  .input-group {
    width: 90%;
  }

  form {
    width: 100%;
    padding: 0;
    margin: 0;
    box-shadow: none;
    border-radius: 0;
  }

}
</style>
