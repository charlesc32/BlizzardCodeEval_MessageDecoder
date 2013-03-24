var maxHeaderLength = 7;
var maxHeaderBinary = 3;

function toBinary(k, key_len) {
    var bin_pos_amt = [1], bin_key = [];
    for (var i = 1; i < key_len; i++) { bin_pos_amt[i] = bin_pos_amt[i - 1] * 2; }
    for (var i = key_len - 1; i >= 0; i--) { if (bin_pos_amt[i] > k) { bin_key.push(0); } else { bin_key.push(1); k -= bin_pos_amt[i]; } }
    if (k > 0) alert('binary conversion err: number exceeds bit type');
    return bin_key.join('');
}

function fromBinary(digits, _len) {
    digits = String(digits); if (digits.length != _len) alert('integer conversion err: bit type mismatch');
    var bin_pos_amt = [1], _num = 0;
    for (var i = 1; i < _len; i++) { bin_pos_amt[i] = bin_pos_amt[i - 1] * 2; }
    for (var i = 0; i < _len; i++) { if (digits.charAt(_len - 1 - i) == 1) _num += bin_pos_amt[i]; }
    return _num;
}

function end_seq(work_len) { var seq = ''; for (var i = 0; i < work_len; i++) { seq += String(1); } return seq; }

function Init_Dec(header, message) 
{
    if (header.length > maxHeaderLength) alert('decode err: invalid header specified');
    var keys = [], cur_key = 0, key_len = 1, seg_num_keys = 1; 		//generate the scheme from header
    for (var i = 0; i <= header.length - 1; i++) {
        keys[i] = toBinary(cur_key, key_len);

        cur_key++;
        if (cur_key == seg_num_keys) { cur_key = 0; seg_num_keys *= 2; seg_num_keys += 1; key_len++; }

    }

    var chars = [], work_pos = 0, work_len = 0, work_str = '';

    for (var i = 0; i <= message.length - 1; i++) {

        work_pos++; work_str += String(message.charAt(i));
        if (work_len == 0 && work_pos == maxHeaderBinary) { work_len = fromBinary(work_str, maxHeaderBinary); work_pos = 0; work_str = ''; }


        if (work_len != 0) {
            if (work_pos == work_len) {
                if (work_str == end_seq(work_len)) {
                    work_len = 0; work_pos = 0; work_str = '';
                } else {
                    chars.push(header.charAt(keys.indexOf(work_str))); work_pos = 0; work_str = '';
                }
            }
        }
    }

    document.write(chars.join('')); alert(chars.join(''));
}