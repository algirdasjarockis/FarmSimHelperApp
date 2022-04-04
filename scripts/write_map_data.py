import json
import xml.etree.ElementTree as et

with open("fields.json") as json_file:
    data = json.load(json_file)
    rootEl = et.Element("fields")

    for item in data['data']['fields']:       
        fieldEl = et.SubElement(rootEl, "field")
        fieldEl.attrib["id"] = str(item['name'])
        fieldEl.attrib["size"] = str(item['ha_size'])
        fieldEl.attrib["price"] = str(item['price'])

    tree = et.ElementTree(rootEl)

    fname = "fields_%s.xml" % str(data['data']['code'])
    tree.write(fname, short_empty_elements=True, xml_declaration=True, encoding="utf-8")

    print("Written to %s" % fname)